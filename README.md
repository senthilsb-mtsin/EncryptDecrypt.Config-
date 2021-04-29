# Encrypt Decrypt app.Config or web.config with Machine Key
Encrypt Decrypt app.config or web.config 

C# Console Program to Encrypt or Decrypt .net config file section

You can Encrypt sections which has sensitive information like passwords,servernames,share paths etc. with **aspnet_regiis.exe**

# Usage

Download the console application from the Binary Folder

```
WebconfigED.exe <path to web.config or app.config>
```

```
WebconfigED.exe "c:\myweb\web.config" 
or 
WebconfigED.exe "c:\myapp\WebconfigED.exe.config"
```

###Note

```app.config``` will be backed up in the same folder before the operation. **aspnet_regiis.exe** only works if the file name is ```web.config```   

## License

[MIT](https://opensource.org/licenses/MIT)
