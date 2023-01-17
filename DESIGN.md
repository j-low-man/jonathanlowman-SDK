# jonathanlowman-SDK
## C# .Net Library SDK for The One API.

### Service

The service was built with only one file.  This would likely be revised to split out portions as necessary in later iterations.

The service centers around a generic method to call The One API and return the needed container/object set.  Helper methods are built around the generic method in order to easily request the data.

Overload functions are available that take a Dictionary<String,String> map of parameters that mirror the api's query string capabilities somewhat.  In order to apply a parameter an entry into the dictionary will be required.

Some key value pair examples that can be used that correlate to the API:

Limit:
    * key = "limit", value = "100"

Page
    * key = "page", value = "2"

Offset
    * key = "offset", value = "3"

Sort:
    * key = "sort", value = "name:asc"

Exists:
    * key = "name", value = ""

Not Exists:
    * key = "!name", value = ""

Less Than, Greater Than, Equal
    * key = "budgetInMillions", value = "<100"
    * key = "academyAwardWins", value = ">0"
    * key = "name", value = "The Two Towers"


The dicitionary structure was set up so that the flexibility of the API remained in tact.  This could of course be simplified so that users of the SDK would be able to use a set of attributes instead of the dictionary for ease of use in the future.