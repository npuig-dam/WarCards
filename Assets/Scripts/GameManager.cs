using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.API.Model;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> startingDeckPrefabs;

    public List<GameObject> deckPrefabs;

    public List<GameObject> allExistingCards;


    public GameAPIClient apiClient;

    public int universalPlayerId;

    void Awake()
    {
   
    

        //Si és el primer cop que es crea, guarden la instància i li assignem DontDestroyOnLoad
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);

        
            if (apiClient == null)
            {
                apiClient = GetComponent<GameAPIClient>();
            } 


            ResetDeck();
            SetAllActive();
      
        }
        //Si és una altre GameManager l'eliminem perquè només volem un. (Patró Singleton)
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Every time you change scene, we wipe and refill the active deck
        ResetDeck();
        
    }

    public void ResetDeck()
    {
        deckPrefabs = new List<GameObject>(startingDeckPrefabs);
    }

    public void AddNewCardById(int id)
    {
        foreach (GameObject cardPrefab in allExistingCards)
        {
            Cards cardScript = cardPrefab.GetComponent<Cards>();
            if (id == cardScript.cardId)
            {
                startingDeckPrefabs.Add(cardPrefab);
                break;
            }
        }
    }

    public void SetAllActive()
    {
        foreach (GameObject g in startingDeckPrefabs)
        {
            Cards card = g.GetComponent<Cards>();
            card.inTheMainHand = true;
            Debug.Log("A funcionat la conversio");
        }
    }

    public void SetAllInnactive()
    {
        foreach (GameObject g in deckPrefabs)
        {
            Cards card = g.GetComponent<Cards>();
            card.inTheMainHand = false;
            Debug.Log("A funcionat la conversio inversa");
        }
    }

    public void RemoveACard(int id)
    {
        foreach (GameObject card in startingDeckPrefabs)
        {
            Cards cardScript = card.GetComponent<Cards>();

            if (id == cardScript.cardId)
            {
                startingDeckPrefabs.Remove(card);
                deckPrefabs.Remove(card);
                SaveCurrentDeckToDb();
                break;
            }
        }
    }

    public async Task<bool> TryLogin(string user, string pass)
    {
        
        Debug.Log("provant el login de usuari"+user);

        //Vale, com que no funcionaba el Await amb les corrutines, he hagut d'utilizar aquesta variable
        // que de forma resumida es un variable que espera a una aplicacio de resultat
        var tcs = new TaskCompletionSource<bool>();

        StartCoroutine(apiClient.GetAllLogins((allLogins) => 
        
        {
            Debug.Log("api respon"); 

            if (allLogins == null)
            {
                Debug.LogError("llista buida");
                tcs.SetResult(false);
            }

            
           
            MLogin foundUser = allLogins.Find(u =>
                u.name.Trim().ToLower() == user.Trim().ToLower() &&
                u.passwd.Trim() == pass.Trim()
            );

            if (foundUser != null)
            {
                Debug.Log("usuari trobat");
                this.universalPlayerId = foundUser.id;
                LoadUserData(this.universalPlayerId);
                tcs.SetResult(true);
            }
            else
            {
                Debug.LogError("usuari no existeix joder");
                tcs.SetResult(false);
            }
        }));

        //El que fara, es que fins que no s'executi un SetResult, no podra ver el return
        return await tcs.Task;
    }
    public async Task<bool> TryNewUser(string user)
    {
        
        Debug.Log("provant nom "+user);

        //Vale, com que no funcionaba el Await amb les corrutines, he hagut d'utilizar aquesta variable
        // que de forma resumida es un variable que espera a una aplicacio de resultat
        var tcs = new TaskCompletionSource<bool>();

        StartCoroutine(apiClient.GetAllLogins((allLogins) => 
        
        {
            Debug.Log("La api reacciona"); 

     
           
            MLogin foundUser = allLogins.Find(u =>
                u.name.Trim().ToLower() == user.Trim().ToLower()
            );

            if (foundUser == null)
            {
                Debug.Log("Nom disponible");
                tcs.SetResult(true);
            }
            else
            {
                Debug.LogError("Aquest nom ja existeix");
                tcs.SetResult(false);
            }
        }));

        //El que fara, es que fins que no s'executi un SetResult, no podra ver el return
        return await tcs.Task;
    }

    public void CreateNewUser(string username, string password)
    {
        Debug.Log("inici de creacio");

        MLogin newUser = new MLogin(username, password);

      
        StartCoroutine(apiClient.CreateLogin(newUser, () => {

       
            StartCoroutine(apiClient.GetAllLogins((allLogins) =>
            {
          

                MLogin foundUser = allLogins.Find(u =>
                    u.name.Trim().ToLower() == username.Trim().ToLower() &&
                    u.passwd.Trim() == password.Trim()
                );

                if (foundUser != null)
                {
                    int newUserId = foundUser.id;
                    Debug.Log("id player " + newUserId);

                 
                    MPlayer newPlayer = new MPlayer(newUserId,newUserId, "1,1,1,1");
                    StartCoroutine(apiClient.CreatePlayer(newPlayer));
                }
                else
                {
                    Debug.LogError("no trobaaaat");
                }
            }));
        }));
    }



    public void LoadUserData(int loginId)
    {
        StartCoroutine(apiClient.GetPlayer(loginId, (playerData) =>
        {
            if (playerData != null && !string.IsNullOrEmpty(playerData.deck))
            {
                startingDeckPrefabs.Clear();
                string[] cardIds = playerData.deck.Split(',');

                foreach (string idStr in cardIds)
                {
                    if (int.TryParse(idStr, out int id))
                    {
                        AddNewCardById(id);
                    }
                }
                ResetDeck();
                SetAllActive();
                Debug.Log("tot carregat");
            }
        }));
    }


    public void SyncDeckFromDb(MPlayer data)
    {

        if (data == null || string.IsNullOrEmpty(data.deck))
        {
            Debug.LogWarning("no hi ha baralla tot hi que no pot ser null per collons");
            return;
        }

     
        startingDeckPrefabs.Clear();

   
        string[] ids = data.deck.Split(',');

        foreach (string idStr in ids)
        {
            if (int.TryParse(idStr, out int cardId))
            {
           
                AddNewCardReal(cardId);
            }
        }

    
        ResetDeck();
        SetAllActive();


    }


    public void AddNewCardReal(int id)
    {
        foreach (GameObject cardPrefab in allExistingCards)
        {
            Cards cardScript = cardPrefab.GetComponent<Cards>();
            if (cardScript != null && id == cardScript.cardId)
            {
                startingDeckPrefabs.Add(cardPrefab);
       
                break;
            }
        }
        SaveCurrentDeckToDb();
    }

    public void SaveCurrentDeckToDb()
    {
        List<string> idList = new List<string>();
        foreach (GameObject cardObj in startingDeckPrefabs)
        {
            Cards script = cardObj.GetComponent<Cards>();
            idList.Add(script.cardId.ToString());
        }

        string deckString = string.Join(",", idList);


        MPlayer updatedPlayer = new MPlayer(universalPlayerId, universalPlayerId, deckString)
        {
            idPlayer = universalPlayerId,
            deck = deckString
        };

        Debug.Log("actualitza el player");
        StartCoroutine(apiClient.UpdatePlayer(universalPlayerId, updatedPlayer));
    }
}

