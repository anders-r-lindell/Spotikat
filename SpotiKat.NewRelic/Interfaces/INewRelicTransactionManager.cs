using System;

namespace SpotiKat.NewRelic.Interfaces {
    public interface INewRelicTransactionManager {
        void AddCustomParameter(string key, IConvertible value);
        void NoticeError(Exception exception);
    }
}