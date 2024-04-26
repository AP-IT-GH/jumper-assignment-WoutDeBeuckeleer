 # Documentatie jumper exercise #
Inleiding
Voor dit project hebben we ons gericht op het ontwikkelen van een zelflerende agent die in staat is om een bewegend obstakel te ontwijken door erover te springen, terwijl hij ook beloningen ontvangt voor het vangen van objecten die over hem vliegen. De agent wordt getraind met behulp van reinforcement learning-technieken om zijn reacties te optimaliseren.
Door het gebruik van Unity's ML-Agents Toolkit wordt de agent blootgesteld aan een dynamische en uitdagende leeromgeving. In elke episode krijgt het obstakel een willekeurige snelheid, waardoor de agent adaptief moet zijn en zijn strategieën voortdurend moet aanpassen.
Het uiteindelijke doel van dit project is om een agent te creëren die zowel obstakels op de grond kan vermijden als bonuspunten kan verdienen door vliegende objecten te vangen, waardoor zijn totale prestaties in de omgeving worden gemaximaliseerd.
Omschrijving
Initieel unity environment
Zodra je begint met het opzetten van de Unity-omgeving, start je met een lege scene. Om het doel van de opdracht te bereiken, is het noodzakelijk om een agent, een obstakel en een beloning te hebben.
Voor het reward te presenteren, kan je gebruik maken van een Sphere. Zorg ervoor dat de sphere in bezit is van een sphere collider. Een sphere collider is nodig om detectie van botsingen met andere objecten te vergemakkelijken en om te bepalen wanneer de agent het reward heeft bereikt. Verder mag je nog een aangepaste tag toe voegen, genaamd 'reward', en deze mag je toepassen aan het reward.
Voor het aanmaken van het obstakel, kan je gebruik maken van een Cube. Ook het obstakel moet in bezit zijn van een collider, in dit geval een box collider. De collider is hier nodig om botsingen met de agent te detecteren, zodat de agent kan reageren en het obstakel kan ontwijken. Voeg eveneens een aangepaste tag 'obstacle' toe en pas deze toe op het object.
Voor het creëren van de agent gebruik je een Cube met een Box Collider. Dit zorgt ervoor dat de agent fysiek interageert met de omgeving en detecteert wanneer het in aanraking komt met obstakels of beloningen.
We kunnen de agent ook al het script 'Behavior Parameters' geven. De Behavior name mag je hier al aanpassen en veranderen zodat dit overeenkomt met de naam van je object (dit zal later ook belangrijk zijn dat het overeenkomt met je naam in je .yaml bestand). Ook het script ‘Decision Requester’ mag al worden toegekend aan de agent, hier moet je niets aan aanpassen. Daarnaast mag ook het script 'Ray Perception Sensor 3D' worden toegewezen aan de agent. Hierbij kun je de 'Rays Per Direction' instellen op de waarde 20. De 'Max Ray Degrees' wordt op 180 gezet omdat we een brede kijkhoek nodig hebben om een goed beeld te krijgen van de omgeving. Als laatste kan je de ray length op 20 zetten om de afstand te specificeren waarmee de agent de omgeving kan scannen en informatie kan verzamelen over obstakels en beloningen.
Als laatste mag er nog een Rigidbody worden toegevoegd aan de agent. Dit is essentieel voor het simuleren van de fysica en de interactie van de agent met de omgeving. De Rigidbody zorgt ervoor dat de agent reageert op krachten en impulsen die erop worden toegepast, zoals zwaartekracht en krachten gegenereerd door beweging of botsingen.
Bij de constraints van de Rigidbody mag je de X- en Z-positie bevriezen. Dit betekent dat de agent niet vrij kan bewegen in de horizontale richtingen, wat nuttig is om te voorkomen dat het van zijn pad afwijkt. Elke rotatie van de agent mag ook worden bevroren, waardoor de agent alleen kan bewegen door te springen of te worden beïnvloed door externe krachten, maar niet door zijn eigen rotatie. Dit helpt om de stabiliteit van de agent te behouden en ongewenste bewegingen te voorkomen.
Het is cruciaal dat de agent en de beloning zich op dezelfde x-as bevinden, omdat de agent beperkt is in voor- en achterwaartse beweging. Om het geheel visueel aantrekkelijker te maken, voeg je een muur en een grond toe achter en onder de objecten, beide gemaakt van een Cube. Dit zorgt voor een beter overzicht van de omgeving.
Beweging obstakel en reward
using UnityEngine;
public class ObjectMover : MonoBehaviour
{
    private float speed; 
    void Start()
    {
        if (gameObject.CompareTag("obstacle"))
        {
            speed = Mathf.RoundToInt(Random.Range(4f, 12f)); 
        }
        else if (gameObject.CompareTag("reward"))
        {
            speed = Mathf.RoundToInt(Random.Range(8f, 10f));
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
Het bovenstaande script, genaamd ObjectMover, is ontworpen om objecten binnen de omgeving te verplaatsen, zoals het obstakel en de beloning.
In de Start methode wordt een variabele genaamd speed geïnitialiseerd, die de snelheid van de verplaatsing van het object bepaalt. Deze snelheid wordt bepaald op basis van de tag die aan het object is toegekend. Als het object de tag "obstacle" heeft, wordt de snelheid ingesteld op een willekeurige waarde tussen 4 en 12 eenheden. Als het object de tag "reward" heeft, wordt de snelheid ingesteld op een willekeurige waarde tussen 8 en 10 eenheden.
Vervolgens wordt in de Update methode het object langs de Z-as verplaatst met een snelheid gelijk aan de waarde van speed, vermenigvuldigd met Time.deltaTime. Dit zorgt ervoor dat de snelheid consistent blijft, ongeacht de framesnelheid van het spel.
Dit script kan worden toegewezen aan zowel het obstakel als de beloning binnen de omgeving. Door de juiste tags aan beide objecten toe te wijzen, wordt het script correct geactiveerd en voeren de objecten de verwachte bewegingen uit. Met dit script kunnen de opgegeven objecten dus dynamisch bewegen in de opgezette unity ruimte.
Verdere uitwerking
Op dit punt hebben we een voltooide omgeving, waarin zowel het obstakel als de beloning zich met een willekeurige snelheid van links naar rechts kunnen bewegen. De parameters voor deze objecten zijn correct ingesteld, waardoor ze dynamisch en uitdagend zijn voor de agent.
Nu is het tijd om het agent script te schrijven, dat de agent zal trainen om het obstakel te vermijden en de beloning te verzamelen.
PlayerAgent script uitwerken
Hier wordt het PlayerAgent script beschreven, dat verantwoordelijk is voor het gedrag van de agent in de omgeving. Het script bepaalt hoe de agent reageert op de omgeving, met als doel obstakels te vermijden en beloningen te verzamelen.
public Transform obstacle;
    public Transform reward;
    public float jumpForce = 10f;
    public float speed = 1f;
    public float obstacleProximityRadius = 6f;
    private Rigidbody rb;
    public GameObject rewardObject;

Als eerst worden er enkele variabelen aangemaakt voor bepaalde eigenschappen te kunnen instantiëren. ‘obstacle’ en ‘reward’ zijn verwijzingen naar de obstakels en beloningen in de omgeving. ‘jumpForce’ bepaalt hoe krachtig de sprong van de agent is. ‘Speed’ geeft de snelheid van de agent aan. ‘obstacleProximityRadius’ bepaalt de straal waarbinnen de agent obstakels kan detecteren. ‘rb’ is een referentie naar de Rigidbody-component van de agent. ‘rewardObject’ is een GameObject dat de beloning vertegenwoordigt.

public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }
In de Initialize() methode initialiseert de agent de waarde van de variabele rb door het RigidBody van de agent op te halen.

public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(0f, 1.5f, 0f);
        obstacle.position = new Vector3(0f, 0.54f, -19.81f);
        reward.GetComponent<Renderer>().enabled = true;
        reward.position = new Vector3(0f, 5.27f, -25.76f);
    }
De OnEpisodeBegin() methode wordt aangeroepen in het begin van een episode. Hier worden de initiële posities van de agent, het obstakel en de beloning ingesteld.

public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(rb.velocity);
        sensor.AddObservation(obstacle.position);
        sensor.AddObservation(obstacle.position - transform.position);
        if (reward != null) {
            sensor.AddObservation(reward.position);
        }
    }
In de CollectObservations() wordt de observaties in de omgeving van de agent verzameld. Dit omvat de positie en snelheid van de agent, de positie van het obstakel, het relatieve verschil tussen de positie van het obstakel en de agent, en indien aanwezig, de positie van de beloning.

public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float distanceToClosestObstacle = float.MaxValue;
        if (obstacle != null)
        {
            float distance = Vector3.Distance(transform.position, obstacle.position);
            if (distance < distanceToClosestObstacle)
            {
                distanceToClosestObstacle = distance;
            }
        }
Hier wordt de afstand tot het dichtstbijzijnde obstakel berekend. Als er een obstakel aanwezig is, wordt de afstand tussen de agent en het obstakel berekend. Als deze afstand kleiner is dan de huidige waarde van distanceToClosestObstacle, wordt deze waarde bijgewerkt naar de kleinere afstand.
       Vector3 controlSignal = Vector3.zero;
 	controlSignal.y = actionBuffers.ContinuousActions[0];
 	transform.Translate(new Vector3(controlSignal.x * 0.1f, controlSignal.y * 	speed, controlSignal.z * 0.1f));
In de bovenstaande code lezen we de actionbuffers uit en letten we op de ContinuousActions[0], die later in de code wordt gebruikt om te bepalen of de agent moet springen. Als deze actie wordt uitgevoerd, moet de agent dus springen. De transform.Translate-functie past de positie van het gameobject aan op basis van deze acties, waarbij de z-as geen invloed heeft op de beweging.
        if (distanceToClosestObstacle < obstacleProximityRadius) {
            if (transform.position.y < 3)
            {
                AddReward(-1f);
                EndEpisode();
            }
        }
Als de agent te dicht bij een obstakel komt en de hoogte van de agent onder een bepaalde drempel ligt (3 in dit geval), wordt een negatieve beloning toegevoegd en eindigt de episode.
        else if (obstacle.position.z > 15)
        {
            AddReward(1f);
            EndEpisode();
        }
    }
Als het obstakel een bepaalde afstand voorbij een bepaald punt op de Z-as (15 in dit geval) beweegt, wordt een positieve beloning toegevoegd en eindigt de episode. Dit kan dienen als een stimulans voor de agent om het obstakel te ontwijken en verder in de omgeving te komen.
In het algemeen wordt de OnActionReceived()aangeroepen wanneer de agent acties ontvangt. Hier wordt bepaald of de agent moet springen, beloningen moet verzamelen of straffen moet ontvangen op basis van de ontvangen input en de huidige toestand van de omgeving.

private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == reward)
        {
            Debug.Log("COLLECT REWARD");
            AddReward(0.5f);
            reward.GetComponent<Renderer>().enabled = false;
        }
    }
De OnCollisionEnter() wordt aangeroepen wanneer de agent botst met een ander object. Als de agent botst met de beloning, wordt een beloning toegekend en wordt de beloning uitgeschakeld.

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continiousActionsOut = actionsOut.ContinuousActions;
 	  continiousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1f : 0;
    }
Als laatste hebben we Heuristics() methode dewelke een heuristiek implementeert die menselijke input nadoet door de agent te laten springen wanneer de spatiebalk wordt ingedrukt. Dit wordt gebruikt voor testdoeleinden tijdens het ontwikkelen en debuggen van het script.

Aanpassingen in unity environment door het script
Na het aanmaken van het Agent script kan dit worden toegewezen aan het agentobject in de Unity Editor.
Bij de parameter van het script sleep je het obstacle object naar het obstacle veld en het reward object naar het veld van het reward. Pas de waarden van JumpForce en Speed aan naar jouw voorkeur. Stel de ObstacleProximityRadius in op ongeveer 2 eenheden. Nadat al deze parameters zijn ingesteld, zou het script correct moeten werken.
Er moet echter nog een andere eigenschap worden aangepast in het script 'Behavior Parameters'. Stel de continious branches in op de waarde 1.
Zodra deze aanpassingen zijn doorgevoerd, zijn alle objecten en scripts correct geconfigureerd. Het project is nu klaar om te beginnen met trainen.
 
Verloop van de training
Resultaat tensorboard
 Om de agent te kunnen trainen, moesten we gebruik maken van een jumper.yaml-bestand, dat zich moet bevinden in een map met de naam config binnen de assets. Het jumper.yaml-bestand wordt aangesproken wanneer een training wordt gestart. Het speelt een cruciale rol bij het configureren van verschillende parameters en hyperparameters die de training van de agent beïnvloeden. Dit omvat zaken als het bepalen van de observatieruimte, actieruimte, het gebruikte neurale netwerk, het trainingsalgoritme en vele andere instellingen die essentieel zijn voor het succesvol trainen van een zelflerende agent. Het is essentieel dat de naam van je agent in het YAML-bestand exact overeenkomt met de naam die je hebt opgegeven in de 'Behavior 
Tensorboard Data:
Je kan zien aan de hand van de omgevingscurve (Environment Curve): Deze curve toont de prestaties van je agent in de omgeving over de tijd.  Dat de  agent snel leert en al vroeg in de training goede resultaten begint te behalen. De reward gaat naar een perfecte score naarmate de training vordert.
Cumulatieve Beloning en Afleveringslengte (Cumulative Reward and Episode Length): Deze grafiek laat zien hoe de cumulatieve beloning en de lengte van afleveringen veranderen gedurende de training. Je kan zien dat de curve lijkt op de omgevingscurve, waarbij de beloning snel stijgt en de afleveringslengte daalt naar een minimum, wat wijst op efficiënte acties van je agent.
Beleidsverlies (Policy Loss): Dit geeft aan hoeveel het beleid van de agent verandert tijdens het trainen. Een lage beleidsverlies duidt over het algemeen op een stabiele training, waarbij het beleid van de agent geleidelijk aan verbetert zonder al te veel fluctuaties.

De samenvatting 
 de trainingsgegevens laat zien dat je agent snel heeft geleerd en al vroeg in het trainingsproces een maximale beloning heeft behaald. Dit suggereert dat de agent efficiënt heeft geleerd om de omgeving te navigeren en beloningen te maximaliseren. De grafieken tonen een snelle stijging van de beloning en een afname van de afleveringslengte, wat wijst op effectieve acties van de agent. 







