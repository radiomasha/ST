using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestUser : MonoBehaviour
{
    [SerializeField] private HandGrabInteractor _righthand;
    [SerializeField] private HandGrabInteractor _lefthand;
    [SerializeField] private TextMeshProUGUI _textmesh;
    [SerializeField] private HandGrabInteractable _napkin;
    [SerializeField] private HandGrabInteractable _spoon;
    [SerializeField] private HandGrabInteractable _saladKnife;
    [SerializeField] private HandGrabInteractable _fishKnife;
    [SerializeField] private HandGrabInteractable _meatKnife;
    [SerializeField] private HandGrabInteractable _saladFork;
    [SerializeField] private HandGrabInteractable _fishFork;
    [SerializeField] private HandGrabInteractable _meatFork;
    [SerializeField] private HandGrabInteractable _dessertSpoon;
    [SerializeField] private HandGrabInteractable _dessertFork;
    [SerializeField] private HandGrabInteractable _waterGlass;
    [SerializeField] private HandGrabInteractable _redWineGlass;
    [SerializeField] private HandGrabInteractable _whiteWineGlass;
    [SerializeField] private HandGrabInteractable _champagneGlass;
    [SerializeField] private ButtonTourThrough _buttonTourThrough;
    [SerializeField] private GameObject _soup;
    [SerializeField] private GameObject _salad;
    [SerializeField] private GameObject _fish;
    [SerializeField] private GameObject _meat;
    [SerializeField] private GameObject _dessert;
    [SerializeField] private GameObject _charger;
    [SerializeField] private GameObject _bread;
    [SerializeField] private GameObject _breadPlate;
    [SerializeField] private GameObject _butterKnife;
    [SerializeField] private Material _waterMaterial;
    [SerializeField] private Material _whiteWineMaterial;
    [SerializeField] private Material _redWineMaterial;
    [SerializeField] private Material _champagneMaterial;
    [SerializeField] private Transform _spoonPos;
   
    public string[] messages =
    {
        "What will you do first?",
        "Right! Put the napkin on your knees",
        "Now they serve soup as a starter.",
        "And what are you going to drink?",
        "Exactly. Now you got a salad.",
        "They are serving white wine",
        "Fish! Ummmm. Yes, still white.",
        "We got steak. And what can be better than a glass of firne red wine with meat.",
        "Perfect time for a dessert!"

    };
    private int _actionsCount = 0;
    private Transform _initWhiteWinePos;
    private GameObject _whiteWine;
    private HashSet<HandGrabInteractable> _processedObjects = new HashSet<HandGrabInteractable>();

    private void Start()
    {
        _initWhiteWinePos = _whiteWineGlass.GetComponentInParent<Transform>();
        _whiteWine = _whiteWineGlass.GetComponentInParent<GameObject>();
        _whiteWine.GetComponent<MeshRenderer>().material = _whiteWineMaterial;
    }

    private void Update()
    {
        _textmesh.text = messages[_actionsCount];
        if (_buttonTourThrough._isCompleted == true)
        {
            _textmesh.gameObject.SetActive(true);
            _textmesh.enabled = true;
            _buttonTourThrough._isCompleted = false;
        }
        Test();
    }

    // Start is called before the first frame update
   
    // Update is called once per frame
    void Test()
    {
        if (_textmesh.text == messages[0])
        {
            if (_righthand.SelectedInteractable == _napkin&& !_processedObjects.Contains(_napkin))
            {
               
                    _napkin.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                    _actionsCount++;
                    StartCoroutine(IncrementActionCount(_napkin));
            }
        }
        if(_textmesh.text == messages[2])
        {
            _soup.SetActive(true);
            _bread.SetActive(true);
            if (_righthand.SelectedInteractable == _spoon && !_processedObjects.Contains(_spoon)) 
            {
                    _spoon.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                    _waterGlass.gameObject.GetComponentInParent<MeshRenderer>().material = _waterMaterial;
                    StartCoroutine(IncrementActionCount(_spoon));
            }
        }
        if (_textmesh.text == messages[3])
        {
            if ( _righthand.SelectedInteractable == _waterGlass&&!_processedObjects.Contains(_waterGlass))
            {
                    _waterGlass.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                    _soup.SetActive(false);
                    StartCoroutine(IncrementActionCount(_waterGlass));
            }
        }
        if(_textmesh.text == messages[4])
        {
            _salad.SetActive(true);
            if (_righthand.SelectedInteractable == _saladKnife && _lefthand.SelectedInteractable == _saladFork&&
                !_processedObjects.Contains(_saladKnife) && !_processedObjects.Contains(_saladFork))
            {
                    _saladKnife.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                    _saladFork.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                    StartCoroutine(IncrementActionCount(_saladKnife));
                    //_processedObjects.Add(_saladFork);
                    ChangeLine(_saladFork);
            }

            _whiteWineGlass.gameObject.GetComponentInParent<MeshRenderer>().material = _whiteWineMaterial;
        }

        if (_textmesh.text == messages[5])
        {
            _salad.SetActive(false);
            _bread.SetActive(false);
            if (_righthand.SelectedInteractable == _whiteWineGlass &&!_processedObjects.Contains(_whiteWineGlass))
            {
                //_actionsCount++;
                _fish.SetActive(true);
                StartCoroutine(IncrementActionCount(_whiteWineGlass));
            }
        }

        if (_textmesh.text == messages[6])
        {
            Instantiate(_whiteWine, _whiteWine.transform.position, _whiteWine.transform.rotation);
            if (_righthand.SelectedInteractable == _fishKnife && _lefthand.SelectedInteractable == _fishFork&&
            !_processedObjects.Contains(_fishKnife) && !_processedObjects.Contains(_fishFork))
            {
                _fishKnife.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                _fishFork.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                StartCoroutine(IncrementActionCount(_fishKnife));
                //_processedObjects.Add(_fishFork);
                ChangeLine(_fishFork);
                _whiteWine.SetActive(false);
            }
           
        }

        if (_textmesh.text == messages[7])
        {   _meat.SetActive(true);
            _redWineGlass.GetComponentInParent<MeshRenderer>().material = _redWineMaterial;
            if (_righthand.SelectedInteractable == _meatKnife && _lefthand.SelectedInteractable == _meatFork &&
                !_processedObjects.Contains(_meatKnife) && !_processedObjects.Contains(_meatFork))
            {
                _meatKnife.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                _meatFork.gameObject.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                StartCoroutine(IncrementActionCount(_meatKnife));
                ChangeLine(_meatFork);
            }

            if (_righthand.SelectedInteractable == _redWineGlass && !_processedObjects.Contains(_redWineGlass))
            {
                _redWineGlass.GetComponentInParent<MeshRenderer>().material.color = Color.green;
                ChangeLine(_redWineGlass);
            }
        }

        if (_textmesh.text == messages[8])
        {
            _charger.SetActive(false);
            _dessert.SetActive(true);
        }
    }  
    private IEnumerator IncrementActionCount(HandGrabInteractable processedObject)
    {
        _actionsCount++;
        yield return new WaitForSeconds(2f);
        _processedObjects.Add(processedObject);
        ChangeLine(processedObject);
    }
    private void ChangeLine(HandGrabInteractable interactable)
    {
        if (interactable != null)
        {
            interactable.gameObject.GetComponentInParent<MeshRenderer>().enabled = false;
            interactable.GetComponentInParent<GameObject>().SetActive(false);
            Destroy(interactable.GetComponentInParent<GameObject>(), 0.5f);
        }
       
    }
}
