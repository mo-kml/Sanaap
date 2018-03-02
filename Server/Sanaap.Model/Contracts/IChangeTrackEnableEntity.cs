﻿using System;

namespace Sanaap.Model.Contracts
{
    public interface IChangeTrackEnableEntity
    {
        DateTimeOffset InsertDate { get; set; }
        DateTimeOffset EditDate { get; set; }
    }
}
