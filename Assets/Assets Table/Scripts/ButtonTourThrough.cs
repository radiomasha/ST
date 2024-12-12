using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class ButtonTourThrough : MonoBehaviour
{
   [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;
        private IInteractableView InteractableView { get; set; }

        [SerializeField]
        private MaterialPropertyBlockEditor _editor;

        [SerializeField]
        private string _colorShaderPropertyName = "_Color";

        [Serializable]
        public class ColorState
        {
            public Color Color = Color.white;
            public AnimationCurve ColorCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            public float ColorTime = 0.1f;
        }

        [Header("Table Elements")]
        [SerializeField]
        private TextMeshProUGUI _textMesh;
        [SerializeField] private TextMeshProUGUI _textMesh2;
        [SerializeField] private GameObject _napkin;
        [SerializeField] private GameObject _charger;
        [SerializeField] private GameObject _spoon;
        [SerializeField] private GameObject _saladKnife;
        [SerializeField] private GameObject _fishKnife;
        [SerializeField] private GameObject _meatKnife;
        [SerializeField] private GameObject _saladFork;
        [SerializeField] private GameObject _fishFork;
        [SerializeField] private GameObject _meatFork;
        [SerializeField] private GameObject _dessertSpoon;
        [SerializeField] private GameObject _dessertFork;
        [SerializeField] private GameObject _breadPlate;
        [SerializeField] private GameObject _butterKnife;
        [SerializeField] private GameObject _waterGlass;
        [SerializeField] private GameObject _redWineGlass;
        [SerializeField] private GameObject _whiteWineGlass;
        [SerializeField] private GameObject _champagneGlass;
        [SerializeField] private GameObject _cutleryQuads;
    

        [SerializeField] private GameObject _pokeInteraction;
        [SerializeField]
        private ColorState _normalColorState = new ColorState() { Color = Color.white };
        //[SerializeField]
        //private ColorState _hoverColorState = new ColorState() { Color = Color.blue };
        [SerializeField]
        private ColorState _selectColorState = new ColorState() { Color = Color.green };
        //[SerializeField]
       // private ColorState _disabledColorState = new ColorState() { Color = Color.grey };
       public bool _isCompleted = false;
        private Color _currentColor;
        private ColorState _target;
        private int _colorShaderID;
        private Coroutine _routine = null;
        private static readonly YieldInstruction _waiter = new WaitForEndOfFrame();
        
        protected bool _started = false;
        //my lines
        private int _clickCount = 0;

        private string[] _messages =
        {
            "Do you have an important celebration in a fancy restaurant, but have no idea what to do with all the tableware? No worries, we have your back.",
            "First of all, napkin. You simply unfold it and put on your knees",
            "This big plate is called 'charger' and it is here for decoration and keeping your cutlery",
            "Now cutlery. This is usually the scariest part. But actually the amount depends on the number of dishes. And pieces are used from outside to the plate. Let's have a closer look",
            "A soup spoon. The form maybe slightly different depends on the type of the soup, but it simply means that soup will be served",
            "Next, a fork and a knife. Probably for a salad. No worries, just follow the flow",
            "Another pair of a fork and a knife. There are special knife and fork for fish, though quite often they will put usual ones",
            "The last set is for a main course. Remember that while cutting your food, you keep a knife in the right hand and a fork in the left. Don't cut your food with a side of the fork",
            "A small plate is for bread. A knife for butter will be on the plate",
            "On the right you will probably see a bunch of glasses. No worries, just take the one a waiter pours for each course",
            "Above the charger there is cutlery for a dessert. Instead of spoon you can sometimes see a knife. Don't cut a dessert with the side of fork",
            "Ok, let's revise what we learnt."
        };
        
        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);

            this.AssertField(InteractableView, nameof(InteractableView));
            this.AssertField(_editor, nameof(_editor));

            _colorShaderID = Shader.PropertyToID(_colorShaderPropertyName);

            UpdateVisual();
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                UpdateVisual();
                InteractableView.WhenStateChanged += UpdateVisualState;
                InteractableView.WhenStateChanged += ChangeTextLines;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= UpdateVisualState;
                InteractableView.WhenStateChanged -= ChangeTextLines;
            }
        }

        private void UpdateVisualState(InteractableStateChangeArgs args)
        {
            UpdateVisual();
        }

        private void ChangeTextLines(InteractableStateChangeArgs args)
        {
           // ChangeText();
        }
        protected virtual void UpdateVisual()
        {
            ColorState target = ColorForState(InteractableView.State);
            if (target != _target)
            {
                _target = target;
                CancelRoutine();
                _routine = StartCoroutine(ChangeColor(target));
            }
        }

        private ColorState ColorForState(InteractableState state)
        {
            switch (state)
            {
                case InteractableState.Select:
                    ChangeText();
                    return _selectColorState;
                //case InteractableState.Hover:
                   // return _hoverColorState;
                case InteractableState.Normal:
                    return _normalColorState;
               // case InteractableState.Disabled:
                //    return _disabledColorState;
                default:
                    return _normalColorState;
            }
        }

        private void ChangeText()
        {
            _textMesh.text = _messages[_clickCount % _messages.Length];
            _clickCount++;
            UpdateTableElements();
        }
        private void UpdateTableElements()
        {
            MeshRenderer _napkinRend = _napkin.GetComponent<MeshRenderer>();
            MeshRenderer _chargerRend= _charger.GetComponent<MeshRenderer>();
            MeshRenderer _spoonRend= _spoon.GetComponent<MeshRenderer>();
            MeshRenderer _saladForkRend= _saladFork.GetComponent<MeshRenderer>();
            MeshRenderer _saladKniferend= _saladKnife.GetComponent<MeshRenderer>();
            MeshRenderer _fishForkRend= this._fishFork.GetComponent<MeshRenderer>();
            MeshRenderer _fishKnifeRend= this._fishKnife.GetComponent<MeshRenderer>();
            MeshRenderer _meatForkRend= this._meatFork.GetComponent<MeshRenderer>();
            MeshRenderer _meatKnifeRend= this._meatKnife.GetComponent<MeshRenderer>();
            MeshRenderer _breadPlateRend= _breadPlate.GetComponent<MeshRenderer>();
            MeshRenderer _butterKnifeRend= _butterKnife.GetComponent<MeshRenderer>();
            MeshRenderer _redWineGlassRend= _redWineGlass.GetComponent<MeshRenderer>();
            MeshRenderer _whiteWineGlassRend= _whiteWineGlass.GetComponent<MeshRenderer>();
            MeshRenderer _waterGlassRend= _waterGlass.GetComponent<MeshRenderer>();
            MeshRenderer _champagneGlassRend= _champagneGlass.GetComponent<MeshRenderer>();
            MeshRenderer _dessForkRend= _dessertFork.GetComponent<MeshRenderer>();
            MeshRenderer _desSpoonRend= _dessertSpoon.GetComponent<MeshRenderer>();
            if (_clickCount == 2)
            {
                _napkinRend.material.color = Color.green;
            }
            else if (_clickCount == 3)
            {
                _chargerRend.material.color = Color.green;
            }
            else if (_clickCount == 4)
            {
                _cutleryQuads.SetActive(true);
            }
            else if (_clickCount == 5)
            {
                _cutleryQuads.SetActive(false);
                _spoonRend.material.color = Color.green;
            }
            else if (_clickCount == 6)
            {
                _saladForkRend.material.color = Color.green;
                _saladKniferend.material.color = Color.green;
            }
            else if (_clickCount == 7)
            {
                _fishForkRend.material.color = Color.green;
                _fishKnifeRend.material.color = Color.green;
            }
            else if (_clickCount == 8)
            {
                _meatForkRend.material.color = Color.green;
                _meatKnifeRend.material.color = Color.green;
            }
            else if (_clickCount == 9)
            {
                _breadPlateRend.material.color = Color.green;
                _butterKnifeRend.material.color = Color.green;
            }
            else if (_clickCount == 10)
            {
                _redWineGlassRend.material.color = Color.green;
                _whiteWineGlassRend.material.color = Color.green;
                _waterGlassRend.material.color = Color.green;
                _champagneGlassRend.material.color = Color.green;
            }
            else if (_clickCount == 11)
            {
                _dessForkRend.material.color = Color.green;
                _desSpoonRend.material.color = Color.green;
            }
            else if (_clickCount == 13)
            {
               _napkinRend.material.color = Color.white;
               _chargerRend.material.color = Color.white;
               _spoonRend.material.color = Color.white;
               _saladKniferend.material.color = Color.white;
               _saladForkRend.material.color = Color.white;
               _fishForkRend.material.color = Color.white;
               _fishKnifeRend.material.color = Color.white;
               _meatForkRend.material.color = Color.white;
               _meatKnifeRend.material.color = Color.white;
               _breadPlateRend.material.color = Color.white;
               _butterKnifeRend.material.color = Color.white;
               _waterGlassRend.material.color = Color.white;
               _redWineGlassRend.material.color = Color.white;
               _whiteWineGlassRend.material.color = Color.white;
               _champagneGlassRend.material.color = Color.white;
               _dessForkRend.material.color = Color.white;
               _desSpoonRend.material.color = Color.white;
               _isCompleted = true;
               StartCoroutine(DeactivateTextPanel());
            }
            
        }

        private IEnumerator DeactivateTextPanel()
        {   _textMesh.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _pokeInteraction.SetActive(false);
            
        }


        private IEnumerator ChangeColor(ColorState targetState)
        {
            Color startColor = _currentColor;
            float timer = 0f;
            do
            {
                timer += Time.deltaTime;
                float normalizedTimer = Mathf.Clamp01(timer / targetState.ColorTime);
                float t = targetState.ColorCurve.Evaluate(normalizedTimer);
                SetColor(Color.Lerp(startColor, targetState.Color, t));

                yield return _waiter;
            }
            while (timer <= targetState.ColorTime);
        }

        private void SetColor(Color color)
        {
            _currentColor = color;
            _editor.MaterialPropertyBlock.SetColor(_colorShaderID, color);
        }

        private void CancelRoutine()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }
        }

        #region Inject

        public void InjectAllInteractableColorVisual(IInteractableView interactableView,
            MaterialPropertyBlockEditor editor)
        {
            InjectInteractableView(interactableView);
            InjectMaterialPropertyBlockEditor(editor);
        }

        public void InjectInteractableView(IInteractableView interactableview)
        {
            _interactableView = interactableview as UnityEngine.Object;
            InteractableView = interactableview;
        }

        public void InjectMaterialPropertyBlockEditor(MaterialPropertyBlockEditor editor)
        {
            _editor = editor;
        }

        public void InjectOptionalColorShaderPropertyName(string colorShaderPropertyName)
        {
            _colorShaderPropertyName = colorShaderPropertyName;
        }

        public void InjectOptionalNormalColorState(ColorState normalColorState)
        {
            _normalColorState = normalColorState;
        }
/*
        public void InjectOptionalHoverColorState(ColorState hoverColorState)
        {
            _hoverColorState = hoverColorState;
        }
*/
        public void InjectOptionalSelectColorState(ColorState selectColorState)
        {
            _selectColorState = selectColorState;
        }

        #endregion
}
