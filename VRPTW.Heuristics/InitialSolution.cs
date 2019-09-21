﻿using System.Collections.Generic;
using System.Linq;
using VRPTW.Helper;
using VRPTW.Model;

namespace VRPTW.Heuristics
{
    public class InitialSolution
    {
        private readonly Dataset _dataset;
        private Customer _depot;
        private List<Customer> _unRoutedCustomers;
        private double _routeMaxCapacity;

        public InitialSolution(Dataset dataset)
        {
            _dataset = dataset;
        }

        private void SetInputData()
        {
            _depot = _dataset.Vertices[0];
            _unRoutedCustomers = _dataset.Vertices.GetRange(1, _dataset.Vertices.Count - 1);
            _routeMaxCapacity = _dataset.Vehicles[0].Capacity;
        }

        public Solution Get()
        {
            SetInputData();
            var routes = new List<Route>();
            while (_unRoutedCustomers.Count > 0)
            {
                var route = new RouteGenerator(_depot, _unRoutedCustomers, _routeMaxCapacity).Generate();
                routes.Add(route);
                _unRoutedCustomers = _unRoutedCustomers.Except(route.Customers).ToList();
            }
            var numberOfRemainingVehicles = _dataset.Vehicles.Count - routes.Count;
            for (var i = 0; i < numberOfRemainingVehicles; i++)
            {
                var customers = new List<Customer>
                {
                    _depot,
                    Helpers.Clone(_depot)
                };
                var route = new Route()
                {
                    Customers = customers,
                    Load = 0.0,
                    Distance = 0.0
                };
                routes.Add(route);
            }
            ResetReturningDepotName(routes);
            return new Solution()
            {
                Routes = routes,
                Cost = routes.Sum(d => d.Distance)
            };
        }

        private void ResetReturningDepotName(List<Route> routes)
        {
            foreach (var r in routes)
            {
                r.Customers.Last().Name = _dataset.Vertices.Count;
            }
        }
    }
}
