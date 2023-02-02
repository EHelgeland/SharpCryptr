# SharpCryptr

SharpCryptr is a small tool for easily encrypting and decrypting files with AES-CBC.

### Usage
The program takes two verb arguments `encrypt` or `decrypt`.  
Further it takes a input file name `-i`, a output file name `-o` and a password `-p`.

Example:
```powershell
.\SharpCryptr.exe encrypt -p foo -i test.txt -o test.enc
```

### Help texts
```powershell
PS> .\SharpCryptr.exe --help
SharpCryptr 1.0.0
Copyright (C) 2023 SharpCryptr

  encrypt    Encrypt your files

  decrypt    Decrypt your files

  help       Display more information on a specific command.

  version    Display version information.

PS> .\SharpCryptr.exe encrypt --help
SharpCryptr 1.0.0
Copyright (C) 2023 SharpCryptr

  -p, --password    Required. The encryption password, make it strong

  -i, --input       Required. Specify the file you want to encrypt/decrypt

  -o, --output      Required. Specify the name of the output file

  --help            Display this help screen.

  --version         Display version information.
```

There is really not much more to it. I wrote it primarily to check out the [Command Line Parser Library](https://github.com/commandlineparser/commandline).  
