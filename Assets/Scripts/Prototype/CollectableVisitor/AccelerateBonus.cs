using System;
using Cysharp.Threading.Tasks;
using Dron;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace NascarSurvival.Collectable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class AccelerateBonus : Collectable
    {
        public bool IsInteracted { get; private set; }
        
        private Collider _collider;
        private Rigidbody _rigidbody;
        private GameStateHandler _gameStateHandler;
        private GameUI _gameUI;


        [Inject]
        private void Init(GameStateHandler gameStateHandler, GameUI gameUI)
        {
            _gameStateHandler = gameStateHandler;
            _gameUI = gameUI;
        }
        
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            Observable.EveryFixedUpdate()
                .Where(_ => _gameStateHandler.CurrentGameState == GameStates.Start)
                .Subscribe(_ => _rigidbody.WakeUp())
                .AddTo(this);
            
            
            _collider.OnCollisionEnterAsObservable()
                .Where(col => col.gameObject.layer == Layers.Hero)
                .Take(1)
                .Subscribe(col => Request(new InteractableAction(), col.gameObject.GetComponentInParent<IInitializer>()))
                .AddTo(this);
        }
        
        public override void Request(IInteractable interactable, IInitializer initializer)
        {
            interactable.Visit(this, initializer.RaceMovement);
            IsInteracted = true;
            gameObject.SetActive(false);
            
            
            if(initializer is EnemyInitializer) return;
            
            _gameUI.SoundHandler.Play("PickUpBonus1");
            GameMessageText(_gameUI, "Speed increased!");
        }

       

        public class Factory : PlaceholderFactory<AccelerateBonus>
        {
            
        }
    }
}