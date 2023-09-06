using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{

    PopUpsManager popUpsManager;
    private Vector2 moveDirection;
    private float moveSpeed;

    [SerializeField]
    Sprite virusSprite;

    [SerializeField]
    Sprite evolutionedVirusSprite;

    void Start(){
        popUpsManager = GameObject.FindWithTag("PopUpsManager").GetComponent<PopUpsManager>();
        moveSpeed=10f;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        CancelInvoke();
        Destroy();
        if(gameObject.name == "EvolutionedVirus" && other.collider.tag=="Player"){
            popUpsManager.StartCoroutine("HackScreen");
        }
    }

    private void OnEnable(){

        //Random virus
        int randomInt = Random.Range(0,10); //0-9

        if(randomInt <= 2){ //30% of spawning an evolutioned virus
            gameObject.GetComponent<SpriteRenderer>().sprite = evolutionedVirusSprite;
            gameObject.name = "EvolutionedVirus";
        }else{
            gameObject.GetComponent<SpriteRenderer>().sprite = virusSprite;
            gameObject.name = "Virus";
        }

        Invoke("Destroy", 5f);
    }

    void Update(){
        transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 dir){
        moveDirection = dir;
    }

    private void Destroy(){
        gameObject.SetActive(false);
    }
}
