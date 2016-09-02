using System;
using SpotiKat.NewRelic.Interfaces;
using NewRelicAgent = NewRelic.Api.Agent;

namespace SpotiKat.NewRelic {
    public class NewRelicTransactionManager : INewRelicTransactionManager {
        public void AddCustomParameter(string key, IConvertible value) {
            try {
                NewRelicAgent.NewRelic.AddCustomParameter(key, value);
            }
            catch {
                // Not much to do...
            }
        }

        public void NoticeError(Exception exception) {
            try {
                NewRelicAgent.NewRelic.NoticeError(exception);
            }
            catch {
                // Not much to do...
            }
        }
    }
}