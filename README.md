# bpq-fbb-test-apps

Apps to test the binary transparency, or otherwise, of BPQ's FBB interface.

Not intended to be used for anything other than proving a point.

Results: FBBPORT is binary-transparent when using the syntax below (no CMDPORT, and using ATTACH rather than CONNECT in the application line). When using CMDPORT and CONNECT, CR becomes CR LF.

## Setup / usage

Far node (node2) bpq32.cfg:

```
PORT
  PORTNUM=9
  ID=Telnet
  DRIVER=Telnet
  CONFIG
    ...
    FBBPORT=8011
    ...
    USER=sysop,rad10,m0lte,,SYSOP
ENDPORT

...

APPLICATION 2,TESTAPP,ATT 9 127.0.0.1 63001,A0BBB-8,FARAPP,255
```

Run `awaynodetestapp` which opens a TCP socket on localhost port 63001, which BPQ connects to.

Connect to the near node (node1), then instruct it to connect to FARAPP, running on the far node - node2

`homenodetestapp` does this, using BPQ's FBB port, and passes bytes to FARAPP.
