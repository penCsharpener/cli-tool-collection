﻿using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Abstractions;
public interface ITextToDataConverter
{
    IAsyncEnumerable<BibleVerseModel> ConvertToBibleReferences();
}
