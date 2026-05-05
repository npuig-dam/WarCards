using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DragObject2D : MonoBehaviour
{
    //Aquest es el script que fa el DragAndDrop 
    //esta posat a cada un dels prefabs de cartes

    //Referencia tant al HandManager i el player (dos GameObjects de l'escena)
    public HandManager handManager;
    public Player player;

    //Referencia la TurnManager que controla el sistema de torns en combat
    public TurnManager turnManager;

    //Referencia al Sprite del Prefab carta
    private SpriteRenderer spriteRenderer;

    //Referencia al propi script pel control de l'efecte de Hover
    private static DragObject2D currentlyHoveredCard;

    //Referencia al control d'animacions (en aquest cas el de la plataforma de joc)
    public PlayZoneAnimationController animController;

    //Referencia al recuadre d'informacio en combat
    public CardInfoUIManager cardInfoUIManager;

    //Referencia al gameManager, present a totes les escenes del joc
    public GameManager gameManager;

    //Referencia a les zones del gestor de baralles
    //Aquestes referencies son nulls a la resta de escenes
    public RemoveCard removecard;
    public AddCard addCard;

    //Referencia a la ma real, dins del gestor de baralles
    public ActualDeckManager actualDeckManager;

    //Variables de moviment del hover
    public float hoverHeight = 0.5f; 
    public float speed = 5f;

    //Variable per la barra d'energia 
    public EnergyBar energyBar; 

    //Variable per accedir al sistema de descartes
    public PileDiscardControl discardControl;

    //Corrutina per el moviment "smooth"
    private Coroutine moveCoroutine;

    //Variable per accedir al sistema de la baralla restant
    public PileRemainControl remainControl;

    //Posicio on apareixen les cartes
    private Vector3 startPosition;
    private Transform startParent;
    private Camera cam;

    //Carta referenciada (el mateix)
    private Cards card;

    //Bool per saber si la carta encara esta siguent agafada
    public bool isDragging { get; private set; }

    //Referencia collider del collider del prefab de la carta
    private Collider2D cardCollider;

    //Variable de vector per simular el hover
    private Vector3 targetPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Guardar la variable start per fer els hovers
        targetPosition = startPosition;
        //Agafa la camera principal
        cam = Camera.main;
        //Inicio tots els components necesaris
        card = GetComponent<Cards>();
        cardCollider = GetComponent<Collider2D>();
        //El player com es un element de l'escena 
        //d'aquesta forma quan la carta apareix busca 
        //un gameObject de la classe Player, nomes n'hi ha un
        //Hi ha alguns d'aquest que seran Nulls a diferents escenes
        player = FindObjectOfType<Player>();
        energyBar = FindObjectOfType<EnergyBar>();
        turnManager = FindObjectOfType<TurnManager>();
        discardControl = FindObjectOfType<PileDiscardControl>();
        remainControl = FindObjectOfType<PileRemainControl>();
        cardInfoUIManager = FindObjectOfType<CardInfoUIManager>();
        animController = FindObjectOfType<PlayZoneAnimationController>();
        addCard = FindObjectOfType<AddCard>();
        removecard = FindObjectOfType<RemoveCard>();
        gameManager = FindObjectOfType<GameManager>();
        actualDeckManager = FindObjectOfType<ActualDeckManager>();
    }
    private void Start()
    {
        //Estableixo la posicio inicial a la posicio del transform del prefab
        //Mes que res es per guardar aquesta posicio per utilitzarla mes tard sense
        // perdre-la
        startPosition = transform.position;
    }


    //Metode que detecta quan es clica la carta
    private void OnMouseDown()
    {
        //Detecta si la coroutina esta activa o no
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        
        //Es modifiquen les varaibles per poder-se moure
        startParent = transform.parent;
        //Sempre que el dragging esta en true es com que esta "clicat"
        isDragging = true;
        transform.SetParent(null);
    }

    //Funcio per modificar la posicio inicial de les cartes
    //Per aplicar un reset de posicio
    public void SetNewHomePostion(Vector3 newHome)
    {
        startPosition = newHome;
    }

    //Metode per arrastrar les cartes
    private void OnMouseDrag()
    {
        //Nomes s'activa despres del MouseDown i simula 
        //un moviment mes o menys fluid, per aconseguir-ho 
        //s'ha de canviar el BodyType del RigidBodyD2 a kinematic
        if (!isDragging) return;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }

    //Metode que detecta quan el mouse pasa per sobre
    private void OnMouseEnter()
    {
        if (isDragging) return;

        //Aquest If s'activa en cas de detectar un hover, per evitar un doble hover en cas
        //d'un clipeix, en teoria amb la separacio que hi ha no hauria de pasar
        if (currentlyHoveredCard != null && currentlyHoveredCard != this) return;

        //S'activa el hover de la carta
        currentlyHoveredCard = this;

        //Es posa la carta amb hover per sobre de les altres per evitar un clipeix de layers
        if (spriteRenderer != null) spriteRenderer.sortingOrder = 100;
        
        //Es para el moviment (para corrutina)
        StopCurrentMovement();

        //S'activa el hover i s'activa la coroutina que provocara aquesta transicio
        // cap al hover, el que fara es moure's durant aquesta coroutina, i quan arribi a cert
        //punt la coroutina parara i la nova posicio sera la del hover (nomes mentre estigui enter)
        Vector3 target = startPosition + new Vector3(0, hoverHeight, 0);
        moveCoroutine = StartCoroutine(MoveCard(target));

        //S'activa el quadre de text amb la informacio del prefab dins del canvas
        if (CardInfoUIManager.Instance != null)
        {
            //Aplica el nom 
            CardInfoUIManager.Instance.UpdateCardTitle(card.name);

            //Aplica les tres informacions, aixo inclou DMG, DEFF i modificadors
            // les cartes nomes poden tindre un maxim de 2 modificadors + el contingut base
            CardInfoUIManager.Instance.UpdateCardInfo1(card.information1); 
            CardInfoUIManager.Instance.UpdateCardInfo2(card.information2); 
            CardInfoUIManager.Instance.UpdateCardInfo3(card.information3); 

            //Per activar el quadre
            CardInfoUIManager.Instance.ToggleInfo(true);
        }
    }

    //Metode que detecta quan el mouse surt de la carta
    private void OnMouseExit()
    {
        //En cas de que estigui arrastrant el aquest metode no s'activa
        //Sembla una broma perque aquesta funcio mai s'hauria d'activar quan hi ha 
        // un dragging, pero si es fa doble clic a la cantonada dreta dels prefabs i al segon clic
        //es deixa mantingut, les dos funcions (la Exit i la Down) s'activen, ara ja no
        if (isDragging) return;


        //Si aquesta carta era la que te el hover activat (en teoria sempre per logica)
        if (currentlyHoveredCard == this)
        {
            //Deixa de ser la carta amb el Hover activat
            currentlyHoveredCard = null;
        }

        //Para la coroutina
        StopCurrentMovement();

        //Inicia una coroutina inversa a la del hover (fa el hover invers)
        // utilitzant la variable startPosition que he guardat al principi
        moveCoroutine = StartCoroutine(MoveCard(startPosition, true));

        //Desactiva el quadre d'informacio de l'escena
        if (CardInfoUIManager.Instance != null)
        {
            CardInfoUIManager.Instance.ToggleInfo(false);
        }
    }

    //Metode per parar la corrutina actual (nomes per evitar problemes entre els Entre-Up i semblants)
    private void StopCurrentMovement()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }

    //Aixo d'aqui es per dir-ho d'alguna forma un mini-update per la corrutina, que nomes funciona mentre la corrutina estigui activa
    //quan la corrutina acaba, aquest update desapareix
    private IEnumerator MoveCard(Vector3 target, bool resettingOrder = false)
    {
        //While mentre la distancia entre l'objectiu i la posicio actual de la carta siguin molt diferents es moura
        //a cada cicle es modifica aquesta distancia per aixi simular un efecte "smooth", es fa perque els floats tenen
        //problemes de precisio molt serios
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            //Es mou la posicio utilitzant velocitat per temps
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            yield return null; 
        }

        //Quan acaba la corrutina es mou la carta a la posicio exacta (en teoria si el While a funcionat be no s'hauria de notar aquest pas)
        transform.position = target;
         
        /*if (resettingOrder)
        {
            handManager.UpdateHandLayout();
        }*/
    }

    //Funcio per fer un update a la posicio una vegada es modifica el nombre de cartes en ma
    public void UpdateBasePosition(Vector3 newWorldPos)
    {
        //La nova posicio inicial sera la nova
        startPosition = newWorldPos;

        //Nomes es mou la carta a la nova posicio si la carta no esta en moviment o en "dragging"
        if (!isDragging && moveCoroutine == null)
        {
            transform.position = startPosition;
        }
    }

    //Metode que s'activa quan es deixa anar la carta
    private void OnMouseUp()
    {
        //Deixa d'estar en dragging i el encara no es jugada (lo de played de predeterminar es fals)
        isDragging = false;
        bool played = false;

        //Primer s'atribueix el collider2D tots els posibles colliders que interactuin entre el limits i la mida del collider
        // del prefab de la carta
        Collider2D[] hits = Physics2D.OverlapBoxAll(cardCollider.bounds.center, cardCollider.bounds.size, 0f);

        //Recorregut de totes les posibles colisions
        foreach (Collider2D hit in hits)
        {
            //Revisa si aquestes zones existeixen a l'escena
            // es per determinar si estem en combat o en edit
            if ((addCard == null) && (removecard == null))
            {
                //S'atribueix la play zone, que es l'unic collider de combat disponible
                PlayZone zone = hit.GetComponent<PlayZone>();

                //Revisa si la zona es correcte i si es el torn del jugador (aixi les cartes dels enemics funcionen diferent)
                if (zone != null && turnManager.playerTurn)
                {
                    //Revisa si es posible tirar la carta per cost d'energia
                    if (card.Cost <= player.currentEnergy)
                    {
                        //En cas de que si, es resta aquesta energia
                        //la carta es jugada, i es crida el metode per
                        //jugarla de veritat
                        player.currentEnergy -= card.Cost;
                        handManager.PlayCard(card);
                        played = true;


                        //De la mateixa forma es desactiva el quadre de text ja que la carta
                        // al jugarla es destruira
                        if (CardInfoUIManager.Instance != null)
                        {
                            CardInfoUIManager.Instance.ToggleInfo(false);
                        }

                        //S'actualitza la barra d'energia del player
                        energyBar.UpdateEnergy(player.currentEnergy);

                        //S'actualitza la baralla de descartes
                        discardControl.UpdateDiscards();

                        //S'actualitza la baralla disponible per robar
                        remainControl.UpdateRemains();

                        //S'activa l'animacio de jugar una carta
                        animController.Activate();
                    }
                    else
                    {
                        //En cas de no poder tirar la carta, en teoria no pasa res, 
                        // i per consequencia la carta tornara al seu lloc
                        Debug.Log("Insufficient energy");
                    }
                }
            }
            else if (hit == addCard.GetComponent<Collider2D>()) 
            {
                //Aquesta part nomes s'activara si estem a l'escena d'edit

                //Revisa si la carta forma part de les cartes de la ma principal
                if (card.inTheMainHand)
                {
                    Debug.Log("Tret de la baralla");

                    //La carta surt de la ma principal
                    card.inTheMainHand = false;

                    //La carta es eliminada
                    actualDeckManager.RemoveACard(card);
                }
                else
                {
                    //En cas de que en lloc de addCard sigui removeCard (si no es un, es l'altre)
                    // es creara un clon presistent per fer la simulacio visual instantanea
                    GameObject persistentClone = actualDeckManager.AddNewCard(card);
                    gameManager.deckPrefabs.Add(persistentClone);



                    // Ara fem les gestions per modificar de forma real la baralla
                    actualDeckManager.handSize = gameManager.deckPrefabs.Count;
                    //Mostrara totes les cartes en el centre de edit
                    actualDeckManager.ShowCard(actualDeckManager.handSize - 1);
                    Debug.Log("Afegit a la baralla");
                    
                    //Fa un reset de les cartes actives per activar les noves
                    gameManager.SetAllActive();

                    //Actualitza el layout de les cartes de edit perque no hi hagi clips
                    actualDeckManager.UpdateDeckLayout();
                }

            }
        }

        //En cas de que la carta no s'hagi jugat per x motiu,
        // igualment si la carta es destrueix no s'activa res
        if (!played)
        {
            //Deixa d'estar "dragging"
            isDragging = false;

            //Para la coroutina de moviment
            StopCurrentMovement();

            //Activa la coroutina per simular l'animacio de tornar a la seva posicio inicial
            moveCoroutine = StartCoroutine(MoveCard(startPosition, true));
        }
    }
}
