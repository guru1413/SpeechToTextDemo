﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextDemo.Interfaces
{
    public interface ISpeechToText
    {
        Task<string> SpeechToTextAsync();
    }
}
