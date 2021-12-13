using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sheep : MonoBehaviour
{
    // [SerializeField] private SheepProperty sheepProperty;
    [SerializeField] private List<SheepProperty> sheepProperty;

    //[SerializeField] private float startSpeed;   
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float force;  
    [SerializeField] private GameObject heartParticlePrefab; //�������� ������
    [SerializeField] private Vector3 sheepOffset;
    [SerializeField] private float jumpForce;

    private Rigidbody rb;
    private BoxCollider bc;
    private MeshRenderer mr;
    private float moveSpeed;
    int randomSheepPropertyIndex;

    [SerializeField] private SoundManager soundManager;

    [SerializeField] private GameEvent SheepDroppedEvent;
    [SerializeField] private GameEvent SheepSavedEvent;


    [SerializeField] private TextMeshProUGUI mysaveSheepText;
     private TextMeshProUGUI mydropSheepText;
   
    [SerializeField] private ScoreManager scoreManager;
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
        scoreManager.Notify += DisplayMessage;                                       // ���������� ������� 
    }
    private void Start()
    {
        randomSheepPropertyIndex = Random.Range(0, sheepProperty.Count);

        //Debug.Log(sheepProperty[randomSheepPropertyIndex].Name); // get
        //sheepProperty[randomSheepPropertyIndex].Name = "Molly"; // set
        //Debug.Log(sheepProperty[randomSheepPropertyIndex].Name); // get

        moveSpeed = sheepProperty[randomSheepPropertyIndex].Speed;
        mr.material = sheepProperty[randomSheepPropertyIndex].Material;
     
    }


    void Update()
    {
        //��������� ��������� � ���� ������ ���� ���� ����
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }


    public void SaveSheep()
    {
        rb.isKinematic = false;
        rb.AddForce(Vector3.up * force);
        ///////////////////////// ��� ������
        moveSpeed = 0;      //1. ��������� �������� ���� �� 0 ��� ��� � ��������!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ������� ��������� ����
        bc.enabled = false; //2. ��������� ���� ��������� ����
        rb.useGravity = false; //3. ��������� ����������
        //4. �������� ������ �� ������� ��� ����� ��� �� �����
        //������� ������ � ������� ���� +
        GameObject particle = Instantiate(heartParticlePrefab, transform.position + sheepOffset, heartParticlePrefab.transform.rotation); // senoPrefab.transform.rotation
        Destroy(particle, 2f);
        Destroy(gameObject, 0.9f);

        soundManager.PlaySheepHitClip();

        SheepSavedEvent.Raise();
       
        scoreManager.AddSaveSheep();
    }


    public void JumpThrowWater()
    {
        //��������� ���������� -��������� �������� - ������ ��� ���� 
        rb.isKinematic = false;
        moveSpeed = 0; //��������� ����
        rb.AddForce(new Vector3(0f, 1f, -1f) * jumpForce);
    }

    public void LandThrowWater()
    {
        //-�������� ���������� - ������������ ��������
        rb.isKinematic = true;
        moveSpeed = sheepProperty[randomSheepPropertyIndex].Speed; //��������� ����
    }


    public void DestroySheep()
    {
        soundManager.PlayDropClip();
        SheepDroppedEvent.Raise();
        Destroy(gameObject);
    }

    //-----------------------------------------------------------
    private void DisplayMessage(int message)
    {
        // mysaveSheepText.text = message.ToString();
        //mydropSheepText.text = message.ToString();
       
        print(message.ToString());
            
    }
    //-----------------------------------------------------------
}
