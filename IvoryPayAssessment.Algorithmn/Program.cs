
using IvoryPayAssessment.Algorithmn.Utilities;

using IvoryPayAssessment.Algorithmn.Utilities;
//int[] socks = { 1, 2, 1, 2, 3, 4, 1, 2, 2, 3 };  // Example array of sock colors
int[] socks = { 10, 20, 20, 10, 10, 30, 50, 10, 20};  // Example array of sock colors

Console.WriteLine("By Sorting-------------------------------------------------------------------------------------------");
Console.WriteLine("Number of pairs of socks with matching colors by Sort: " + PairsOfMatchingColors.sockMerchant(socks));

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
//Console.WriteLine("By Graph----------------------------------------------------------------------------------------------");


//int[] socks = { 1, 2, 1, 2, 3, 4, 1, 2, 2, 3 };  // Example array of sock colors
//Console.WriteLine("Number of pairs of socks with matching colors By Graph: " +WithGraph.DisplayResult. CountSockPairs(socks));


Console.WriteLine("By Searching-------------------------------------------------------------------------------------------");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Number of pairs of socks with matching colors By Search: " + WithSearching. CountSockPairs(socks));

Console.ReadLine ();