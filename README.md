# UndertaleRTLscript
a script of UndertaleModTool for lettering in right-to-left

To trigger right-to-left, run the script in UndertaleModTool. You only have to do it once on a file.
Then start the string with:
(ts:[x],[spacing])
Where [x] is the x pixel-coordinate of the text, and [spacing] is the spacing in pixels between each letter. It takes negative values, so right-to-left is possible.

Example:

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
