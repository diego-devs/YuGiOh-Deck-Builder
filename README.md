por [Diego Diaz](https://github.com/diego-devs)

# YuGiOh! Creador de Decks | Deck Builder
![image](/YGOCardSearch/wwwroot/images/deckbuilder1.png)
<p><img src="/YGOCardSearch/wwwroot/images/logo-header-2.jpg" alt="img" width="300" /></p>

Esta solución consta de dos proyectos: una aplicación de consola llamada **YugiohDB** y una aplicación web ASP.NET llamada **YGOCardSearch**. 

1.  **YGOCardSearch**: Aplicación ASP.NET Core con Razor Pages. Proyecto principal. Buscador de cartas y Creador de Decks. 
2.  **YugiohDB**: Aplicación de consola que nos ayuda a descargar las cartas, imágenes y mapearlas localmente. 

## Contenido

- [YuGiOh! Creador de Decks | Deck Builder](#yugioh-creador-de-decks--deck-builder)
  - [Contenido](#contenido)
    - [Proyecto YugiohDB](#proyecto-yugiohdb)
    - [Proyecto YGOCardSearch](#proyecto-ygocardsearch)
  - [Pre requisitos:](#pre-requisitos)
    - [Instalación de herramientas de Entity Framework y SQL](#instalación-de-herramientas-de-entity-framework-y-sql)
  - [Pasos para ejecutar](#pasos-para-ejecutar)
  - [API Utilizada](#api-utilizada)
  - [Contribuciones](#contribuciones)
  - [Historial de Cambios](#historial-de-cambios)

### Proyecto YugiohDB
El proyecto YugiohDB se creó inicialmente para obtener las cartas y descargar las imágenes, lo que permite crear una base de datos utilizando Migrations de EF Core. Puedes descargar las imágenes y montar la base de datos en tu propia máquina utilizando este proyecto, las instrucciones se encuentran en Program.cs

### Proyecto YGOCardSearch
El proyecto YGOCardSearch es el sitio principal de esta aplicación.
Consta de un Buscador de cartas, un Visualizador de Cartas, un Constructor de Decks o Deck Builder, y próximamente deberá tener un Deck Manager o Administrador de Decks. 

## Pre requisitos:

### Instalación de herramientas de Entity Framework y SQL
La aplicación YugiohDB utiliza Entity Framework Core Migrations para crear y mantener la base de datos.

Asegúrate de tener las herramientas de Entity Framework instaladas globalmente en tu sistema:

```bash
dotnet tool install --global dotnet-ef
dotnet ef
```

## Pasos para ejecutar

1. Clona o haz Fork en el Repositorio.
2. **Actualiza el connection string acorde a tu servidor local en los siguientes archivos:**
    - ``YGOCardSearch/appsettings.json``
    - ``YGOCardSearch/Data/YgoContext.cs``
    - ``YugiohDB/YgoContext.cs``
3. **Crea la base de datos:**
   
    Navega al directorio del proyecto YugiohDB y ejecuta los siguientes comando de **dotnet ef**:

    ```bash
    dotnet ef database update
    ```
    **Esto aplicará las migraciones y creará la base de datos local.**

4. **Cargar todas las cartas de Yugioh.**
   
   Todas las cartas de Yugioh ya se encuentran en este repositorio en la ruta YGOCardSearch/Data/ en los siguientes archivos: 
   - allCards.json
   - allCards.txt
   - ids.txt
  
    Cada uno de estos archivos contiene todas las cartas de Yugioh hasta Septiembre de 2023. 
    
    Puedes seguir estos pasos para descargar las cartas más recientes y actualizadas de la API o puedes omitir esta parte y pasar directamente al paso **'7. Add all cards to database'** usando las cartas que ya tienes en estos archivos. 

    Para cargar todas las cartas actualizadas desde ygoProDeck API sigue el orden del código comentado en Program.cs Main method.
    ```cs
    // 1- Download and save all cards from API
    // 2- Load all cards from json file
    // 3- Download all images and images small first
    // 4- Map images to correct path in local machine
    // 5- Map banlist info
    // 6- Save and overwrite modified cards to local folder
    // 7- Add all cards to database
    ```
    Descomenta el código de program.cs en orden para poder descargar las cartas actualizadas desde ygoprodeck API a tu directorio configurado previamente.

    El último método añadirá todas las cartas de tu path local a la base de datos YgoDB:

    ```cs
    // 7- Add all cards to database
    await YgoProDeckTools.AddAllCards(cardsLocalPath);
    ```

5. **Contruye la Aplicación Web (Proyecto YGOCardSearch)**
Para ejecutar la aplicación web YGOCardSearch, haz build. Puedes hacerlo en VS o por medio de comandos. Personalmente, me gusta más usar la consola y el teclado que el mouse. 

En tu consola favorita navega al directorio del proyecto YGOCardSearch y ejecuta el comando: 

    ```bash
    dotnet build
    ```

    La aplicación web estará disponible en http://localhost:5000 (o http://localhost:5001 si habilitas el modo HTTPS).

¡Listo! Has configurado y construido la solución de **YGOCardSearch**. Si encuentras algún problema durante el proceso, asegúrate de verificar los requisitos del sistema y las dependencias necesarias en la documentación del proyecto.



## API Utilizada
Ambas aplicaciones utilizan la API ygoProDeck para obtener información sobre las cartas de Yu-Gi-Oh!. Puedes consultar la documentación de la API para obtener más detalles sobre su uso.
[ygoProDeck API](https://db.ygoprodeck.com/api-guide/)

## Contribuciones
¡Contribuciones y mejoras son bienvenidas! Si deseas contribuir a este proyecto, por favor sigue las pautas de contribución.
Por favor, si encuentras algún bug o error, siéntete libre de contactarme para resolverlo.

## Historial de Cambios

[Unreleased]
- Pagina de Deck Builder terminada con funcionalidades.

[0.0.1] - 07/09/2023
- Versión inicial del proyecto.
- Funcionalidades básicas de búsqueda y visualización de cartas.

[0.0.2] - 01/08/2024
- Se han resuelto diversos bugs y errores visuales y de backend. 
- El sitio de Deck Builder es funcional para crear y editar decks. Tiene fallos y le hacen falta características aun. 