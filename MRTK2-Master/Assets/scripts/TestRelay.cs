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
    private Text inputObject;

   public Text serverCodeText;

    Button hostButton;
    InputField inputcode;
    Button joinButton;

    Button stopClientButton;
    // Start is called before the first frame update
    public async void Start()
    {
           serverCodeText = GameObject.Find("codedisplay").GetComponent<Text>();
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();



        hostButton = GameObject.Find("Start Host").GetComponent<Button>();
        hostButton.onClick.AddListener(CreateRelay);
        joinButton = GameObject.Find("Join Button").GetComponent<Button>();
        joinButton.onClick.AddListener(JoinRelay);
        // stopClientButton = GameObject.Find("Stop client button").GetComponent<Button>();
        //stopClientButton.onClick.AddListener(StopClient);



    }

    static void setCode(Text text, string code) {
        text.text = code;

    }

     

    public static async void CreateRelay() {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3); // max number of clients
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
         

            //Text serverCodeTest = GameObject.Find("displaycode").GetComponent<Text>();
            //setCode(serverCodeTest, joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
            Debug.Log("Host started");
            Debug.Log("with Joincode " + joinCode);
          
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }

   

    public void StopClient() {
        try {
            NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
        Debug.Log("client disconnected");
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }
        
    }

   


    public async void JoinRelay() {
        try
        {
            //string joinCode = GameObject.Find("inputcode").GetComponent<TextMeshPro>().text;
            TMP_InputField inputfield = GameObject.Find("joininputfield").GetComponent<TMP_InputField>();
            string joinCode = inputfield.text;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
    