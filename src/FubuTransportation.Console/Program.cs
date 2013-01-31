using System;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Impl;
using Rhino.ServiceBus.MessageModules;
using StructureMap;

namespace FubuTransportation.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.Configure(x => x.For<IMessageModule>().Singleton().Use<FubuTransportationMessageModule>());
            new RhinoServiceBusConfiguration()
                .UseStructureMap(container)
                .Configure();

            var inputMessage = new Input{Message = "Test", CorrelationId = Guid.NewGuid()};
            FubuApplication.For<TransportationRegistry>()
                .StructureMap(container)
                .Bootstrap();

            var bus = container.GetInstance<IStartableServiceBus>();
            bus.Start();
            bus.Send(inputMessage);

            System.Console.ReadKey();
        }
    }
}
