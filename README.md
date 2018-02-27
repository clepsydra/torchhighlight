# TorchHighlight
**Highlight areas on the Windows Desktop using your smartphone like a torchlight, e.g. for presentations**

Idea is that you can wave your smartphone and a highlighting circle is shown and moving based on your movement in a transparent top window.

For this there are two parts:

_The Application_: 

It is a WPF based Windows application. It shows a QR code which can be scanned by the Smartphone to get the URL of the hosted Http Listener.

_The Web Page_:

A very small Angular based page which reads the current position from the smartphone and transfers it.


To run the application:
run npm install for the Web application.
Then build it for production using 
ng build --prod

Compile and run the Wpf application.

Scan the QR code on the screen using a smartphone and open the web page.
Point your smartphone to the screen and hit the "On" button on the web page.
That yould make a yellow circle show up and the QR code goes away.
