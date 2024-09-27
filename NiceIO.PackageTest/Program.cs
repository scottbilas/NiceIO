var path =
	
#if ENABLE_NAMESPACE
AnotherNamespace
#else
NiceIO
#endif

.NPath.AnotherStatic.Combine("file.txt");

Console.WriteLine($"Path is {path}");
