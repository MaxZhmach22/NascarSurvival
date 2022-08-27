using System;
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


        [Inject]
        private void Init(GameStateHandler gameStateHandler)
        {
            _gameStateHandler = gameStateHandler;
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
        }
        
        public class Factory : PlaceholderFactory<AccelerateBonus>
        {
            
        }
    }
}