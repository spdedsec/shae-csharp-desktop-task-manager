# shae-csharp-desktop-task-manager — Guía de instalación y uso (Español)

SHAE Task Manager — un gestor de tareas basado en consola escrito en C# (.NET).

Aplicado el **Sistema de Diseño SHAE Field Manual** (a una interfaz de
texto): tablas de tareas monoespaciadas alineadas, tinta sobre papel
conceptual mapeada a la terminal, una señal óxido por encabezado/acción,
sin ruido decorativo. Véase `DESIGN.md`.

## Características

- Agregar tarea (título, descripción, fecha de vencimiento, prioridad)
- Listar tareas (filtrar por estado: pendiente / completada / todas)
- Marcar tarea como completa / incompleta (`toggle`)
- Eliminar tarea (`delete`)
- Guardar/cargar en JSON (`~/.shae-task-manager/tasks.json`)
- Salida de consola formateada

## Requisitos

- .NET SDK 6.0+ (`dotnet`)

> **Nota:** El SDK de .NET no está instalado en este entorno (solo el
> runtime), por lo que la app no puede compilarse ni probarse localmente
> aquí. Véase `scripts/install.sh`.

## Instalación

```bash
git clone https://github.com/spdedsec/shae-csharp-desktop-task-manager.git
cd shae-csharp-desktop-task-manager
./scripts/install.sh
```

## Compilación y ejecución

```bash
cd src
dotnet build
dotnet run
```

## Uso

```
Comandos:
  list          Listar todas las tareas
  todo          Listar tareas pendientes
  done          Listar tareas completadas
  add           Agregar una nueva tarea (interactivo)
  toggle <id>   Marcar una tarea como completa/incompleta
  delete <id>   Eliminar una tarea
  help          Mostrar ayuda
  quit / exit   Salir
```

Las tareas se guardan en `~/.shae-task-manager/tasks.json`.

## Licencia

MIT. Véase `LICENSE`.
