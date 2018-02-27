# torchhighlight
**Highlight areas on the Windows Desktop using your smartphone like a torchlight, e.g. for presentations**

Idea is that you can wave your smartphone and a highlighting circle is shown and moving based on your movement in a transparent top window.

For this there are two parts:

_The Application_: 

It is a WPF based Windows application. It shows a QR code which can be scanned by the Smartphone to get the URL of the hosted Http Listener.

_The Web Page_:

A very small Angular based page which reads the current position from the smartphone and transfers it.

