﻿using System;

namespace VRPTW.Configuration
{
    public static class Config
    {
        public static string GetSolverType()
        {
            return ConfigManager.AppSetting["SolverType"];
        }
        public static string GetDataSource()
        {
            return ConfigManager.AppSetting["DataSource"];
        }

        public static ConnectionString GetConnectionString()
        {
            return new ConnectionString()
            {
                DNS = ConfigManager.AppSetting["ConnectionString:DNS"],
                Port = ConfigManager.AppSetting["ConnectionString:Port"],
                DBName = ConfigManager.AppSetting["ConnectionString:DBName"],
                Username = ConfigManager.AppSetting["ConnectionString:Username"],
                Password = ConfigManager.AppSetting["ConnectionString:Password"]
            };
        }
        public static FileOperation GetFileOperation()
        {
            return new FileOperation()
            {
                FilePath = ConfigManager.AppSetting["FileOperation:FilePath"],
                FileName = ConfigManager.AppSetting["FileOperation:FileName"]
            };
        }

        public static SolverParam GetSolverParam()
        {
            return new SolverParam()
            {
                TimeLimit = Convert.ToDouble(ConfigManager.AppSetting["SolverParam:TimeLimit"]),
                MIPGap = Convert.ToDouble(ConfigManager.AppSetting["SolverParam:MIPGap"]),
                Threads = (int)Convert.ToDouble(ConfigManager.AppSetting["SolverParam:Threads"])
            };
        }

        public static HeuristicsParam GetHeuristicsParam()
        {
            return new HeuristicsParam()
            {
                InitialSolutionParam = GetInitialSolutionParam()
            };
        }

        public static InitialSolutionParam GetInitialSolutionParam()
        {
            return new InitialSolutionParam()
            {
                Alpha1 = Convert.ToDouble(ConfigManager.AppSetting["HeuristicsParam:InitialSolutionParam:Alpha1"]),
                Alpha2 = Convert.ToDouble(ConfigManager.AppSetting["HeuristicsParam:InitialSolutionParam:Alpha2"]),
                Mu = Convert.ToDouble(ConfigManager.AppSetting["HeuristicsParam:InitialSolutionParam:Mu"]),
                Lambda = Convert.ToDouble(ConfigManager.AppSetting["HeuristicsParam:InitialSolutionParam:Lambda"])
            };
        }
    }
}