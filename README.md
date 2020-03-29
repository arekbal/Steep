### Steep
Is performance oriented dotnet package.
It comes with a bunch of data types helping capable developers write 'faster' programs.
This objective is being achieved in many forms and with varying impact:
1. SList is meant to fill 80% of roles List<> takes while keeping most of original behavior, 
  but significantly improving performance for most use cases.
2. Option and Result are there to promote robust returns and replace nulls and throwing exceptions.
3. Vec is like a SList but for dynamically resizable unmamaged memory. Where managed types are unnecesarry,
 more can be achieved with custom small object allocator and plain alloc/free.
4. StrideVec, StrideSpan are there for procesor cache friendly iteration operations.
5. Enumerators are meant to replace some of use cases of System.Linq with lighter substitution.
6. Promise is a lighter (memory wise) version of System.Threading.Task.
7. Str family of types is for (mostly) stack based short string manipulation. 

All improvements are (meant to be) extensively benchmarked.

Couple of techniques used to achieve improvements:
- changes in APIs so moves and borrows are allowed and safety is sometimes achieved by different means(entry, two step API) 
  and sometimes burden is pushed onto developer(SList.MoveIn()).
- ignoring less common or legacy use cases.
- using modern Lang features like ref returns and ref locals.
- by not using relatively expensive abstractions (readonly fields, virtual overloading, exceptions).
- using vectorisation where applicable.
- by providing flyweight(Promise) versions of considerably bloated types(Task). 


[Docs](https://github.com/arekbal/Steep/tree/dev/docs)
