rem  This batch program creates the service from the published exe
rem  eg sc create ExampleServiceNew binPath="C:\\SqliteConsole\\bin\\publish\\SqliteConsole.exe"
rem  needs to be run with elevated privlidges

sc create <ServiceName> binPath="<AbsolutePublishedServicePath>"
sc description <ServiceName> "This is the description of the service."