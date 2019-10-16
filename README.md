## passt
password store - simple password manager for Windows (commandline)

## What is this?
This is a as-simple-as-it-can-get password store, similar to the famous "pass - the standard unix password manager", which I use when I'm 
using Linux.

## Why did you do this?
Mostly because I wanted to do some finger-stretching in C# and because I don't want to run `pass` inside the WSL.

## How to use it?
All passwords are stored at your User directory (i.e., `%HOMEDRIVE%%HOMEPATH%`) inside the file `.pass`.
No matter which command you are using, if this file does not exists it will be silently created.

Adding a new password with a descriptive $TITLE:
```
passt.exe add $TITLE
Username: $USER
Password: **********
```

Remove a password from the store:
```
passt.exe remove $TITLE
```

Get a password from the store and copy it directly to the clipboard:
```
passt.exe get $TITLE
```

That's it. It will not get any more complex or blown-up with features.

## Security
The `.pass` file is en- and decrypted via the `System.IO.File.{Encrypt|Decrypt}` methods from 
mscorlib.dll, netstandard.dll, System.IO.FileSystem.dll (see [Microsoft's documentation](https://docs.microsoft.com/en-us/dotnet/api/system.io.file.encrypt?view=netframework-4.8)
for more info).

To make things short: Only the user account, which encrypted the file can decrypt it. This is done via
the cryptographic service provider (CSP).

### Limitations
The CSP is **not** available in Home versions of Windows. The filesystem **must** be formatted as NTFS.

## License
Licensed under the terms and conditions of the MIT license. See LICENSE for more information.
