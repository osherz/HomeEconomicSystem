﻿using Graphviz4Net.Graphs;
using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.Model;
using HomeEconomicSystem.Utils;
using Stateless;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    internal class AssociationRulesVM : IInnerVM<States, Triggers>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private StateMachine<States, Triggers> _stateMachine;
        private AssociationsModel _associationsModel;

        private ObservableCollection<RuleVM> _rules;
        public ObservableCollection<RuleVM> Rules
        {
            get { return _rules; }
            set { SetProperty(ref _rules, value); }
        }


        private Graph<IEnumerable<Product>> _graph;
        public Graph<IEnumerable<Product>> Graph
        {
            get => _graph;
            set => SetProperty(ref _graph, value);
        }

        private bool _isGraph;

        public bool IsGraph
        {
            get { return _isGraph; }
            set { SetProperty(ref _isGraph, value); }
        }

        private bool _isTable;

        public bool IsTable
        {
            get { return _isTable; }
            set { SetProperty(ref _isTable, value); }
        }

        public ICommand SwitchVisualization { get; set; }

        public event EventHandler<string> InnerStateChanged;

        public AssociationRulesVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property) );
            IsGraph = true;
            IsTable = false;
            _associationsModel = new AssociationsModel();
            InitAssociationRules();
        }

        private void InitAssociationRules()
        {
            Graph = new Graph<IEnumerable<Product>>();
            Rules = _associationsModel.AssosiatonRules
                .Select(rule=>new RuleVM(rule))
                .ToObservableCollection();
            foreach (var rule in _associationsModel.AssosiatonRules)
            {
                var a = rule.Product;
                var b = rule.GoesWith;
                Graph.AddVertex(a);
                Graph.AddVertex(b);

                var edge = new Edge<IEnumerable<Product>>(a, b, new Arrow()) { Label = rule.Probability.ToString()};
                Graph.AddEdge(edge);
            }

            
        }

        public IReadOnlyDictionary<States, Action> GetStatesEntryAction()
        {
            return new Dictionary<States, Action>
            { 
                {States.AssociationRules, ()=> _stateMachine.Fire(Triggers.Load)},
                {States.AssociationRulesGraph, ()=>IsGraph = true },
                {States.AssociationRulesTable, ()=>IsTable = true },
            };
        }

        public IReadOnlyDictionary<States, Action> GetStatesExitAction()
        {
            return new Dictionary<States, Action>
            {
                {States.AssociationRulesGraph, ()=>IsGraph = false },
                {States.AssociationRulesTable, ()=>IsTable = false },
            };
        }

        public void SetStateMachine(StateMachine<States, Triggers> stateMachine)
        {
            _stateMachine = stateMachine;
            SwitchVisualization = stateMachine.CreateCommand(Triggers.Switch);
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}