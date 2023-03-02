using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour
{
    
    [SerializeField]
    private Button hostButton, joinButton;

    [SerializeField]
    private TMP_InputField joinInputField, hostCodeDisplay;
    
    public async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostButton.onClick.AddListener(CreateRelay);
        joinButton.onClick.AddListener(JoinRelay);

    }

    public async void CreateRelay() {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); // max number of clients
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
            );
            
            hostCodeDisplay.text = joinCode;

            NetworkManager.Singleton.StartHost();
            Debug.Log("Host started");
            Debug.Log("with Joincode " + joinCode);
          
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

    public async void JoinRelay() {
        try
        {
            string joinCode = joinInputField.text;
            Debug.Log("joining relay with code " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(joinAllocation.RelayServer.IpV4,
            (ushort) joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData

            );

            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }

    }
}
    