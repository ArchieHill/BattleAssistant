// BattleFile.cs
//
// Copyright (c) 2022 Archie Hill
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattleAssistant.Common.CustomValidation
{
    public class BattleFile : ValidationAttribute
    {
        private const int FirstNumPosSubtractor = -3;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string path = value as string;
            string fileName = Path.GetFileNameWithoutExtension(path);
            string fileNumbers = fileName.Substring(fileName.Length + FirstNumPosSubtractor);
            
            if(!File.Exists(path))
            {
                return new("Could not find battle file");
            }
            else if(Path.GetExtension(path) != ".ema")
            {
                return new("The file is not a .ema battle file");
            }
            else if(!Regex.IsMatch(fileNumbers, @"\d\d\d"))
            {
                return new("The file name doesn't end with three numbers e.g 001");
            }
            return ValidationResult.Success;
        }
    }
}
