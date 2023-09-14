# bpq-fbb-test-apps

Apps to test the binary transparency, or otherwise, of BPQ's FBB interface.

Not intended to be used for anything other than proving a point.

Results: FBBPORT is binary-transparent apart from CR which it transmits as CR LF.

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
    CMDPORT 63001
ENDPORT

...

APPLICATION 2,TESTAPP,C 9 HOST 0 S,A0BBB-8,FARAPP,255
```

Run `awaynodetestapp` which opens a TCP socket on localhost port 63001, which BPQ connects to.

Connect to the near node (node1), then instruct it to connect to FARAPP, running on the far node - node2

`homenodetestapp` does this, using BPQ's FBB port, and passes bytes to FARAPP.
