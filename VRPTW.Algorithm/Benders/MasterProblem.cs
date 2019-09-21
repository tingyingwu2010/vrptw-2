﻿using Gurobi;
using System;
using System.Collections.Generic;
using System.Text;
using VRPTW.Configuration;
using VRPTW.Helper;
using VRPTW.Model;

namespace VRPTW.Algorithm.Benders
{
    class MasterProblem
    {
        private GRBEnv _env;
        private GRBModel _model;
        private int _status;
        private List<List<List<GRBVar>>> _vehicleTraverse;
        private GRBVar _z;       

        private List<Vehicle> _vehicles;
        private List<Customer> _vertices;
        
        public MasterProblem(GRBEnv env, List<Vehicle> vehicles, List<Customer> vertices)
        {
            _vehicles = vehicles;
            _vertices = vertices;
            _model = new GRBModel(env);
            _env = env;
            Generate();
        }

        public GRBModel GetModel()
        {
            return _model;
        }

        public void Generate()
        {
            InitializeModel();
            SetSolverParameters();
            InitializeDecisionVariables();
            CreateBinaryDecisionVariables();
            CreateGeneralDecisionVariables();
            CreateObjective();
        }

        private void InitializeModel()
        {
            _model = new GRBModel(_env);
        }

        private void SetSolverParameters()
        {
            _model.Parameters.TimeLimit = Config.GetSolverParam().TimeLimit;
            _model.Parameters.MIPGap = Config.GetSolverParam().MIPGap;
            _model.Parameters.Threads = Config.GetSolverParam().Threads;
        }

        private void InitializeDecisionVariables()
        {
            _vehicleTraverse = new List<List<List<GRBVar>>>();
            _z = new GRBVar();
        }

        private void CreateBinaryDecisionVariables()
        {
            for (int v = 0; v < _vehicles.Count; v++)
            {
                List<List<GRBVar>> vehicleTraverseV = new List<List<GRBVar>>();
                for (int s = 0; s < _vertices.Count; s++)
                {
                    List<GRBVar> vehicleTraverseC = new List<GRBVar>();
                    for (int e = 0; e < _vertices.Count; e++)
                    {
                        vehicleTraverseC.Add(_model.AddVar(0, 1, 0, GRB.BINARY, ""));
                    }
                    vehicleTraverseV.Add(vehicleTraverseC);
                }
                _vehicleTraverse.Add(vehicleTraverseV);
            }
        }

        private void CreateGeneralDecisionVariables()
        {
            _z = _model.AddVar(double.MinValue, double.MaxValue, 0, GRB.CONTINUOUS, "");
        }

        private void CreateObjective()
        {
            var cost = new GRBLinExpr();
            cost.AddTerm(1.0, _z);
            _model.SetObjective(cost, GRB.MINIMIZE);
        }
    }
}