namespace 
#if ENABLE_NAMESPACE
AnotherNamespace
#else
NiceIO
#endif
;

partial class NPath
{
	public static NPath AnotherStatic => new("a/b/c");
}
