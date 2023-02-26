# Foobar

A Linux-like operating system made using cosmos, a framework that compiles C# code to x86 code.

## Installation

Download git and run the following command:

```
git clone https://github.com/4xvoid/OsmiumOS
```

And thats all! now you can work on the project all you want or even compile it and boot it from a USB.

## Using OsmiumLang

### Hello World!

Here is a simple OsmiumLang hello world script:

```c
void main
{
    print "Hello World!\n"
}

goto main
```

### If-Else Statements

Here is an example of if statements in OsmiumLang:

```c
a = 10
b = 5

if a == b
{
    print "10 == 5"
}
else
{
    print "10 != 5"
}
```

### While Statments

Here is an example of some while loops:

```c
a = 0
while a <= 10
{
    a = a + 1

    if a == 5
    {
        break
    }
} 
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.