# Windows Optimizer C#
**Designed by:** avk013  
**Developed by:** Cline mokoki

A hybrid Windows performance optimization tool for Windows 7/8/10/11.

## Features
- **Hybrid architecture**: Modular Core library with separate CLI and WPF GUI frontends
- **Preset profiles**: Safe, Balanced, Strict, Gaming modes
- **Custom profiles**: Create, save, and load JSON-based optimization templates
- **Safety first**: Registry backup before changes, PIN protection (default: 111111), admin rights enforcement
- **Cross-version support**: Automatic OS detection and version-specific tweaks

## Usage
Run with Administrator privileges.
```bash
# CLI
WindowsOptimizer.Cli.exe [Safe|Balanced|Strict|Gaming] [PIN]

# GUI
WindowsOptimizer.Gui.exe
```

## Security
- All system modifications require PIN confirmation
- Automatic registry backup to `Backups/ComputerName_OS_Date_Time/`
- Admin rights check on startup
- Detailed operation logging

## Requirements
- Windows 7/8/10/11 x64
- .NET 7 Runtime (or self-contained build)
- Administrator privileges

## License
Copyright (c) avk013. Project Architect: avk013. Implementation: Cline mokoki
