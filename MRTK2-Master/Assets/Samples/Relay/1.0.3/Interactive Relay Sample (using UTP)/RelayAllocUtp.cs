using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class RelayAllocUtp : MonoBehaviour
{
    public Text PlayerIdText;
    public Text SelectedRegion;
    public Text HostAllocationIdText;
    public Text JoinCodeText;
    public Text PlayerAllocationIdText;
    public InputField JoinCodeInput;
    private Guid hostAllocationId;
    private Guid playerAllocationId;
    private string joinCode = "n/a";
    private string playerId = "n/a";
    private string regionId = "n/a";
    private List<string> regions = new List<string>();
    private Allocation hostAllocation;
    private NativeList<NetworkConnection> serverConnections;
    private NetworkConnection clientConnection;
    private bool isRelayServerConnected = false;

    public NetworkDriver HostDriver;
    public NetworkDriver PlayerDriver;

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerId = AuthenticationService.Instance.PlayerId;
        serverConnections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
        StartCoroutine(UpdateUI());
    }

    private void OnApplicationQuit()
    {
        serverConnections.Dispose();
        serverConnections = default;
    }

    //Substitute for proper UI handling.
    private IEnumerator UpdateUI()
    {
        while (true) 
        {
            PlayerIdText.text = playerId;
            SelectedRegion.text = regionId;
            HostAllocationIdText.text = hostAllocationId.ToString();
            JoinCodeText.text = joinCode;
            PlayerAllocationIdText.text = playerAllocationId.ToString();
            yield return new WaitForSeconds(1f);
        }
    }

    // Update the NetworkDrivers regularily to ensure the host/player is kept online.
    void Update()
    {
        if (HostDriver.IsCreated && isRelayServerConnected)
        {
            HostUpdate();
        }

        if (PlayerDriver.IsCreated && clientConnection.IsCreated) 
        {
            ClientUpdate();
        }
    }

    void OnDestroy()
    {
        HostDriver.Dispose();
        PlayerDriver.Dispose();
    }

    public void OnHost() 
    {
        //Disabling the ability to run multiple hosts on the same instance.
        if (HostDriver.IsCreated && isRelayServerConnected) 
        {
            Debug.Log("Host already active. Server listening for inbound connections.");
            return;
        }

        StartCoroutine(StartServerAndJoin());
    }

    public void OnJoin() 
    {
        if (playerAllocationId != Guid.Empty)
        {
            Debug.Log("Client already active and connected to a Host. Disconnect from the current server before attempting to join another.");
            return;
        }
        StartCoroutine(ClientBindAndConnect(JoinCodeInput.text));
    }

    public void OnDisconnect() 
    {
        if (playerAllocationId == Guid.Empty) 
        {
            Debug.Log("Not currently connected to a server.");
            return;
        }

        PlayerDriver.Disconnect(clientConnection);
        playerAllocationId = Guid.Empty;
    }

    public void HostUpdate() 
    {
        HostDriver.ScheduleUpdate().Complete();

        // Clean up stale connections
        for (int i = 0; i < serverConnections.Length; i++)
        {
            if (!serverConnections[i].IsCreated)
            {
                serverConnections.RemoveAtSwapBack(i);
                --i;
            }
        }

        //Accept incoming client connections
        NetworkConnection incomingConnection;
        while ((incomingConnection = HostDriver.Accept()) != default(NetworkConnection))
        {
            serverConnections.Add(incomingConnection);
            Debug.Log("Accepted an incoming connection.");
        }

        //Process events from all connections
        for (int i = 0; i < serverConnections.Length; i++)
        {
            Assert.IsTrue(serverConnections[i].IsCreated);

            NetworkEvent.Type eventType;
            while ((eventType = HostDriver.PopEventForConnection(serverConnections[i], out _)) != NetworkEvent.Type.Empty)
            {
                if (eventType == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    serverConnections[i] = default(NetworkConnection);
                }
            }
        }
    }

    public void ClientUpdate() 
    {
        PlayerDriver.ScheduleUpdate().Complete();

        //Resolve event queue
        NetworkEvent.Type eventType;
        while ((eventType = clientConnection.PopEvent(PlayerDriver, out _)) != NetworkEvent.Type.Empty)
        {
            if (eventType == NetworkEvent.Type.Connect)
            {
                Debug.Log("Client connected to the server");
            }
            else if (eventType == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                clientConnection = default(NetworkConnection);
            }
        }
    }

    private IEnumerator StartServerAndJoin() 
    {
        yield return StartCoroutine(StartRelayServer());
        StartCoroutine(ClientBindAndConnect(joinCode));
    }

    // Launch this method as a coroutine
    private IEnumerator StartRelayServer()
    {
        // Request an allocation to the Relay service
        var relayMaxPlayers = 5;
        var allocationTask = RelayService.Instance.CreateAllocationAsync(relayMaxPlayers);
        Debug.Log("Attempting to get Relay service host allocation...");

        while (!allocationTask.IsCompleted)
        {
            yield return null;
        }

        if (allocationTask.IsFaulted)
        {
            Debug.LogError("Create allocation request failed");
            yield break;
        }

        hostAllocation = allocationTask.Result;
        hostAllocationId = hostAllocation.AllocationId;
        Debug.Log($"Host allocated. Host Allocation Id: {hostAllocationId}");

        yield return RequestJoinCode(hostAllocation);

        // Format the server data, based on desired connectionType
        var relayServerData = HostRelayData(hostAllocation, "udp");

        // Create the network parameters using the Relay server data
        var relayNetworkParameter = new RelayNetworkParameter { ServerData = relayServerData };

        // Bind and listen to the Relay server
        yield return ServerBindAndListen(relayNetworkParameter);
    }

    // Launch this method as a coroutine
    private IEnumerator RequestJoinCode(Allocation allocation) 
    {
        // Request the join code to the Relay service
        var joinCodeTask = RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log("Attmepting to retrieve Join Code...");

        while (!joinCodeTask.IsCompleted)
        {
            yield return null;
        }

        if (joinCodeTask.IsFaulted)
        {
            Debug.LogError("Create join code request failed");
            yield break;
        }

        // Get the Join Code, you can then share it with the clients so they can join
        joinCode = joinCodeTask.Result;
        Debug.Log($"Join Code recieved. Join Code: {joinCode}");
    }

    // Launch this method as a coroutine
    private IEnumerator ServerBindAndListen(RelayNetworkParameter relayNetworkParameter)
    {
        // Create the NetworkSettings with Relay parameters
        var networkSettings = new NetworkSettings();
        networkSettings.AddRawParameterStruct(ref relayNetworkParameter);

        // Create the NetworkDriver using NetworkSettings
        HostDriver = NetworkDriver.Create(networkSettings);

        // Bind the NetworkDriver to the local endpoint
        if (HostDriver.Bind(NetworkEndPoint.AnyIpv4) != 0)
        {
            Debug.LogError("Server failed to bind");
        }
        else
        {
            // The binding process is an async operation; wait until bound
            while (!HostDriver.Bound)
            {
                HostDriver.ScheduleUpdate().Complete();
                yield return null;
            }

            // Once the driver is bound you can start listening for connection requests
            if (HostDriver.Listen() != 0)
            {
                Debug.LogError("Server failed to listen");
            }
            else 
            {
                isRelayServerConnected = true;
            }
        }

        Debug.Log("Server bound.");
    }

    private IEnumerator ClientBindAndConnect(string relayJoinCode)
    {
        // Send the join request to the Relay service
        var joinTask = RelayService.Instance.JoinAllocationAsync(relayJoinCode);
        Debug.Log("Attempting to join allocation with join code...");

        while (!joinTask.IsCompleted)
            yield return null;

        if (joinTask.IsFaulted)
        {
            Debug.LogError("Join Relay request failed");
            Debug.LogException(joinTask.Exception);
            yield break;
        }

        // Collect and convert the Relay data from the join response
        var allocation = joinTask.Result;
        playerAllocationId = allocation.AllocationId;
        Debug.Log($"Player allocated with allocation Id: {playerAllocationId}");

        // Format the server data, based on desired connectionType
        var relayServerData = PlayerRelayData(allocation, "udp");
        var relayNetworkParameter = new RelayNetworkParameter { ServerData = relayServerData };

        // Create the NetworkSettings with Relay parameters
        var networkSettings = new NetworkSettings();
        networkSettings.AddRawParameterStruct(ref relayNetworkParameter);

        // Create the NetworkDriver using the Relay parameters
        PlayerDriver = NetworkDriver.Create(networkSettings);

        // Bind the NetworkDriver to the available local endpoint.
        // This will send the bind request to the Relay server
        if (PlayerDriver.Bind(NetworkEndPoint.AnyIpv4) != 0)
        {
            Debug.LogError("Client failed to bind");
        }
        else
        {
            while (!PlayerDriver.Bound)
            {
                PlayerDriver.ScheduleUpdate().Complete();
                yield return null;
            }

            // Once the client is bound to the Relay server, you can send a connection request
            clientConnection = PlayerDriver.Connect(relayNetworkParameter.ServerData.Endpoint);
        }
    }

    public static RelayServerData HostRelayData(Allocation allocation, string connectionType = "udp")
    {
        // Select endpoint based on desired connectionType
        var endpoint = GetEndpointForConnectionType(allocation.ServerEndpoints, connectionType);
        if (endpoint == null)
        {
            throw new Exception($"endpoint for connectionType {connectionType} not found");
        }

        // Prepare the server endpoint using the Relay server IP and port
        var serverEndpoint = NetworkEndPoint.Parse(endpoint.Host, (ushort)endpoint.Port);

        // UTP uses pointers instead of managed arrays for performance reasons, so we use these helper functions to convert them
        var allocationIdBytes = ConvertFromAllocationIdBytes(allocation.AllocationIdBytes);
        var connectionData = ConvertConnectionData(allocation.ConnectionData);
        var key = ConvertFromHMAC(allocation.Key);

        // Prepare the Relay server data and compute the nonce value
        // The host passes its connectionData twice into this function
        var relayServerData = new RelayServerData(ref serverEndpoint, 0, ref allocationIdBytes, ref connectionData,
            ref connectionData, ref key, connectionType == "dtls");
        relayServerData.ComputeNewNonce();

        return relayServerData;
    }

    public static RelayServerData PlayerRelayData(JoinAllocation allocation, string connectionType = "udp")
    {
        // Select endpoint based on desired connectionType
        var endpoint = GetEndpointForConnectionType(allocation.ServerEndpoints, connectionType);
        if (endpoint == null)
        {
            throw new Exception($"endpoint for connectionType {connectionType} not found");
        }

        // Prepare the server endpoint using the Relay server IP and port
        var serverEndpoint = NetworkEndPoint.Parse(endpoint.Host, (ushort)endpoint.Port);

        // UTP uses pointers instead of managed arrays for performance reasons, so we use these helper functions to convert them
        var allocationIdBytes = ConvertFromAllocationIdBytes(allocation.AllocationIdBytes);
        var connectionData = ConvertConnectionData(allocation.ConnectionData);
        var hostConnectionData = ConvertConnectionData(allocation.HostConnectionData);
        var key = ConvertFromHMAC(allocation.Key);

        // Prepare the Relay server data and compute the nonce values
        // A player joining the host passes its own connectionData as well as the host's
        var relayServerData = new RelayServerData(ref serverEndpoint, 0, ref allocationIdBytes, ref connectionData,
            ref hostConnectionData, ref key, connectionType == "dtls");
        relayServerData.ComputeNewNonce();

        return relayServerData;
    }

    private static RelayServerEndpoint GetEndpointForConnectionType(List<RelayServerEndpoint> endpoints, string connectionType)
    {
        foreach (var endpoint in endpoints)
        {
            if (endpoint.ConnectionType == connectionType)
            {
                return endpoint;
            }
        }

        return null;
    }

    private static RelayAllocationId ConvertFromAllocationIdBytes(byte[] allocationIdBytes)
    {
        unsafe
        {
            fixed (byte* ptr = allocationIdBytes)
            {
                return RelayAllocationId.FromBytePointer(ptr, allocationIdBytes.Length);
            }
        }
    }

    private static RelayConnectionData ConvertConnectionData(byte[] connectionData)
    {
        unsafe
        {
            fixed (byte* ptr = connectionData)
            {
                return RelayConnectionData.FromBytePointer(ptr, RelayConnectionData.k_Length);
            }
        }
    }

    private static RelayHMACKey ConvertFromHMAC(byte[] hmac)
    {
        unsafe
        {
            fixed (byte* ptr = hmac)
            {
                return RelayHMACKey.FromBytePointer(ptr, RelayHMACKey.k_Length);
            }
        }
    }
}
