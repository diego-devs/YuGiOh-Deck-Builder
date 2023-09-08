por [Diego Diaz](https://github.com/diego-devs)

# YuGiOh! TCG Card Search
![img](/YGOCardSearch/wwwroot/images/logo-header-2.jpg)
<!-- Si tienes un logo, reemplaza la URL -->

Este proyecto consta de dos aplicaciones .NET: una aplicación de consola llamada YugiohDB y una aplicación web ASP.NET llamada YGOCardSearch. Ambas aplicaciones están diseñadas para ayudarte a buscar cartas del juego de cartas coleccionables Yu-Gi-Oh! y construir y administrar Decks.

## Contenido

- [YuGiOh! TCG Card Search](#yugioh-tcg-card-search)
  - [Contenido](#contenido)
  - [Introducción](#introducción)
    - [Proyecto YugiohDB](#proyecto-yugiohdb)
    - [Proyecto YGOCardSearch](#proyecto-ygocardsearch)
  - [Prerequisites:](#prerequisites)
    - [Instalacion de herramientas de EF](#instalacion-de-herramientas-de-ef)
  - [Pasos para ejecutar](#pasos-para-ejecutar)
  - [API Utilizada](#api-utilizada)
  - [Contribuciones](#contribuciones)
  - [Historial de Cambios](#historial-de-cambios)

## Introducción
Este repositorio alberga dos proyectos .NET destinados a facilitar la búsqueda y construcción de mazos de cartas de Yu-Gi-Oh! TCG.

### Proyecto YugiohDB
El proyecto YugiohDB se creó inicialmente para obtener las cartas y descargar las imágenes, lo que permite crear una base de datos utilizando **Entity Framework Migrations**. Los desarrolladores pueden descargar las imágenes y montar la base de datos en su propia máquina utilizando este proyecto.

### Proyecto YGOCardSearch
El proyecto YGOCardSearch es el sitio principal de esta aplicación.

## Prerequisites:

### Instalacion de herramientas de EF
La aplicación YugiohDB utiliza Entity Framework Core Migrations para crear y mantener la base de datos.

Asegúrate de tener las herramientas de Entity Framework instaladas globalmente en tu sistema:

```bash
dotnet tool install --global dotnet-ef
dotnet ef
```

## Pasos para ejecutar

1. Clona o haz Fork en el Repositorio.
2. **Actualiza el connection string en:**
    - ``YGOCardSearch/appsettings.json``
    - ``YGOCardSearch/Data/YgoContext.cs``
    - ``YugiohDB/YgoContext.cs``
3. **Crea la base de datos:**
   
    Navega al directorio del proyecto YugiohDB y ejecuta los siguientes comando de dotnet ef:

    ```bash
    cd YuGiOhTCG/YugiohDB
    dotnet ef database update
    ```
    Esto aplicará las migraciones y creará la base de datos local.

4. **Cargar todas las cartas de Yugioh.**
   
   Todas las cartas de Yugioh ya se encuentran en este repositorio en la ruta YGOCardSearch/Data/ en los siguientes archivos: 
   - allCards.json
   - allCards.txt
   - ids.txt
  
    Cada uno de estos archivos contiene todas las cartas de Yugioh de septiembre/2023. Puedes seguir estos pasos para descargar las cartas más recientes y actualizadas de la API o puedes omitir esta parte y pasar directamente al paso **'7. Inserta todas las cartas en BD'** usando las cartas que ya tienes en estos archivos. 

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

    El último método añadirá todas las cartas de tu path local a la base de datos YgoDB.

    ```cs
    // 7- Add all cards to database
    await YgoProDeckTools.AddAllCards(cardsLocalPath);
    ```

5. **Contruye la Aplicación Web (Proyecto YGOCardSearch)**
Para ejecutar la aplicación web YGOCardSearch, sigue estos pasos:
Navega al directorio del proyecto YGOCardSearch y ejecuta la aplicación web utilizando el siguiente comando:

    ```bash
    dotnet build
    ```

    La aplicación web estará disponible en http://localhost:5000 (o http://localhost:5001 si habilitas el modo HTTPS).


¡Listo! Has configurado y construido la solución de YuGiOhTCG. Si encuentras algún problema durante el proceso, asegúrate de verificar los requisitos del sistema y las dependencias necesarias en la documentación del proyecto.

## API Utilizada
Ambas aplicaciones utilizan la API ygoProDeck para obtener información sobre las cartas de Yu-Gi-Oh!. Puedes consultar la documentación de la API para obtener más detalles sobre su uso.
[ygoProDeck API](https://db.ygoprodeck.com/api-guide/)

## Contribuciones
¡Contribuciones y mejoras son bienvenidas! Si deseas contribuir a este proyecto, por favor sigue las pautas de contribución.
Por favor, si encuentras algún bug o error, siéntete libre de contactarme para resolverlo.

## Historial de Cambios

[Unreleased]
- Pagina de Deck Builder terminada con funcionalidades.
- Pagina de Deck Management terminada con funcionalidades.

[0.0.1] - 07/09/2023
- Versión inicial del proyecto.
- Funcionalidades básicas de búsqueda y visualización de cartas.