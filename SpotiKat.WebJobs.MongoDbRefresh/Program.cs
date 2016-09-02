using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Bootstrap;
using Bootstrap.Autofac;
using SpotiKat.Services.Interfaces;

namespace SpotiKat.WebJobs.MongoDbRefresh {
    internal class Program {
        private static void Main(string[] args) {
            Bootstrapper.With.Autofac().Start();

            while (true) {
                var task1 = Task1();
                task1.Wait();

                var task2 = Task2();
                task2.Wait();

                Console.WriteLine("Sleep: Start [{0}]", DateTime.Now);
                Thread.Sleep(new TimeSpan(0, 0, 20, 0));
                Console.WriteLine("Sleep: End [{0}]", DateTime.Now);
            }
        }

        private static async Task Task1() {
            try {
                Console.WriteLine("Task1: Start [{0}]", DateTime.Now);
                var lastAlbumService = ((IContainer) Bootstrapper.Container).Resolve<ILastAlbumService>();
                await lastAlbumService.GetFeedItemsAlbumsAsync(FeedItemSource.Boomkat, 1);
                Console.WriteLine("Task1: End [{0}]", DateTime.Now);
            }
            catch (Exception ex) {
                Console.WriteLine("Task1 failed: {0}", ex.Message);
            }
        }

        private static async Task Task2() {
            try {
                Console.WriteLine("Task2: Start [{0}]", DateTime.Now);
                var lastAlbumService = ((IContainer) Bootstrapper.Container).Resolve<ILastAlbumService>();
                await lastAlbumService.GetFeedItemsAlbumsAsync(FeedItemSource.Sbwr, 1);
                Console.WriteLine("Task2: End [{0}]", DateTime.Now);
            }
            catch (Exception ex) {
                Console.WriteLine("Task2 failed: {0}", ex.Message);
            }
        }
    }
}