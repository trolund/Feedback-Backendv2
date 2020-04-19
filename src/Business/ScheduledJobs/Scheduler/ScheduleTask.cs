// using System;
// using System.Threading.Tasks;
// using Microsoft.Extensions.DependencyInjection;

// namespace Business.Scheduler {
//     public class ScheduleTask : ScheduledProcessor {

//         public ScheduleTask (IServiceScopeFactory serviceScopeFactory) : base (serviceScopeFactory) { }

//         protected override string Schedule => "*/10 * * * *";

//         public override Task ProcessInScope (IServiceProvider serviceProvider) {
//             Console.WriteLine ("Processing starts here");
//             return Task.CompletedTask;
//         }
//     }
// }