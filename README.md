# ParticleTerminal

Simulador de chuva com física rodando direto no terminal, feito em C#. As gotas caem do topo da tela com gravidade real, respondem ao vento e têm densidade ajustável em tempo real via teclado.

## Requisitos

- [.NET 8+](https://dotnet.microsoft.com/download)
- Terminal com suporte a ANSI/cores RGB (funciona no GitHub Codespaces, macOS Terminal, Windows Terminal)

## Instalação

```bash
git clone https://github.com/seu-usuario/ParticleTerminal
cd ParticleTerminal
dotnet restore
```

## Como rodar

```bash
dotnet run
```

## Controles

| Tecla | Ação |
|-------|------|
| `↑` | Aumenta a densidade da chuva |
| `↓` | Diminui a densidade da chuva |
| `→` | Vento para a direita |
| `←` | Vento para a esquerda |
| `Q` | Sair |

O HUD na parte inferior da tela mostra o estado atual do vento e da densidade em tempo real.

## Arquitetura

O projeto é dividido em três camadas independentes:

```
ParticleTerminal/
├── Particle.cs          # Entidade: posição, velocidade, tempo de vida
├── ParticleWorld.cs     # Simulação: gerencia partículas e aplica forças
├── Forces.cs            # Forças: Gravity, Wind (interface IForce)
├── TerminalRenderer.cs  # Renderização: converte física em ASCII + cores ANSI
└── Program.cs           # Game loop + input do teclado
```

A física (`Particle`, `ParticleWorld`, `IForce`) é completamente desacoplada da renderização. É possível trocar o `TerminalRenderer` por um renderer gráfico (SkiaSharp, OpenGL) sem tocar em nada da simulação.

## Dependências

- [Spectre.Console](https://spectreconsole.net/) — output colorido no terminal

## Próximos passos

- [ ] Colisão das gotas com o chão (respingo)
- [ ] Modo neve (partículas com deriva lateral aleatória)
- [ ] Modo tempestade (rajadas de vento intermitentes)
- [ ] Exportar um frame como arquivo de texto