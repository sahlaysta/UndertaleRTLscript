# UndertaleRTLscript
a script of UndertaleModTool for lettering in right-to-left

To use right-to-left, run the script [RTLv2.csx](https://github.com/sahlaysta/UndertaleRTLscript/blob/main/RTLv2.csx) in UndertaleModTool. You only have to do this once.


Start any string that you want in right-to-left with: `(ts:[x],[spacing])` where `x` is the exact pixel-location of the text, and [spacing] is the spacing in pixels between each letter. You will have to figure these out yourself, but these examples may help you:

Examples:

```
(ts:200,-8)* (The shadow of the ruins&  looms above^1, filling you with&  determination.)/
```
<img src ="https://i.imgur.com/GzLmY2S.png"/>

<hr>
Another example:

```
(ts:177,-9)See that heart^1? &That is your SOUL^1,&the very culmination&of your being!/
```
<img src = "https://i.imgur.com/tB5X3sB.png"/>

<hr>
Another example:

```
(ts:440,-16)* You encountered the Dummy.
```
<img src = "https://i.imgur.com/D9hC9Qm.png"/>

<hr>
Sans example:

```
(ts:200,-10)\E4* do you wanna have&  a bad time?/
```
<img src = "https://i.imgur.com/Ipv11hn.png"/>
