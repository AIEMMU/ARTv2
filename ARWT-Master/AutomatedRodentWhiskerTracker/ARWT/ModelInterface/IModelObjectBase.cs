﻿using ARWT.Model;

namespace ARWT.ModelInterface
{
    public interface IModelObjectBase
    {
        string ErrorMessage
        {
            get;
            set;
        }

        ModelObjectState ModelObjectState
        {
            get;
        }

        void Commit();
        void MarkAsDeleted();
        void DataLoadComplete();
    }
}
