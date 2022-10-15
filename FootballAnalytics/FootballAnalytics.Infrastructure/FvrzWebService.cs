﻿using FootballAnalytics.Application;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Domain.Model;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace FootballAnalytics.Infrastructure
{
    public class FvrzWebService : IFvrzWebService
    {
        private readonly IConfiguration _configuration;

        public FvrzWebService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IEnumerable<FetchedGame> FetchGames()
        {
            // TODO: Error Handling
            var url = _configuration["NapoliGamePlanUrl"];
            var web = new HtmlWeb();
            var doc = web.Load(url);

            return HtmlParser.ParseGamePlan(doc);
        }
    }
}