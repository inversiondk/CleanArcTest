# CleanArcTest - overvejelser

POC på en løst koblet DDD-arkitektur baseret på Clean Architecture med inspiration fra CQRS. 

Formålet har været at opbygge en ny arkitektur som erstatning for en entity-service baseret løsning, der efterhånden er vokset ud over sine rammer. 
Den primære overvejelse har været at skabe et setup, hvor der kan benyttes forskellige tilgange til queries og commands i forhold til bagvedliggende persisterings-teknologi. 
Den eksisterende løsning er 100% baseret på EF (.Net Framework-udgaven) til både read og writes. Dette kan til tider skabe en performance bottleneck. Ét af de primære formål har derfor været at undersøge mulighederne for at splitte dette ud på f.eks.: 
- Dapper til læsninger (queries)
- EF Core til skrivninger (commands)

Et CQRS light setup taler godt ind i dette ønske, da det primære formål her, er at separere commands og queries. 

### Repository og Unit of Work patterns
Der er mange holdninger til repository og Unit of Work patterns. De primære argumenter man oftest støder på er: 
1) Repository og Unit of Work er allerede bygget ind i DbContext i Entity Framework - så det giver ingen mening at bygge en ekstra abstraktion ovenpå disse. 
2) Brugen af generiske repository klasser giver ingen mening, men er blot med til at øge enten kompleksitet eller fjerne fleksibilitet

Pkt. 2 er jeg helt enig i. Det bryder også med godt DDD at udstille generiske metoder til at "QueryAll" etc. I stedet bør repositories overholde ubiquitous language - og dermed bør repository metoder fortælle præcist hvad de gør. 
Pkt. 1 er lidt mere subjektivt. På den ene side er det fuldstændig korrekt, at EF igennem DbContext udstiller repositories via sine DbSets. Det er også korrekt, at change tracking i EF fungerer som Unit of Work da EF vil holde styr på hvilke ændringer som skal committes. I hvert fald hvis man har et nogenlunde solidt design. 
Problemet opstår hvis man f.eks. vil forsøge at overholde DDD-princippet om KUN at gemme aggregate roots - og altså ikke bare fortage vilkårlige CRUD operationer på alle entiteter uden at tænke på domæne-regler. 
I dette tilfælde er det ønskværdigt at have noget i stil med IWriteRepository<T> where T : IAggregateRoot. Og har man først indført et separat repository pattern, så er man også nødt til at indføre Unit of Work til at håndtere commits m.v. 
En anden overvejelse med brugen af DbContext som Repository og UoW går på at man udstiller den komplette DbContext. Dette betyder fuld adgang til den bagvedlæggende database-konfiguration etc. Dermed melder spørgsmålet sig: Er det en god idé at implementere en abstration simpelthen med det formål at begrænse adgang / funktionalitet til dét man har brug for?
  
I denne POC er der lavet forsøg med at opdele repositories i ICommandRepository - som altid opererer på en IAggregateRoot samt IQueryRepository - som opererer på Entity (base entity). Formålet med denne opsplitning er at kunne lave en Dapper implementering på IQueryRepository-implemnteringerne (se f.eks. TrademarkQueryRepository.cs) samt en ren EF implementering til ICommandRepository (se f.eks. TrademarkCommandRepository.cs). 

### Virker det så i virkeligheden?
Både og. Hvis man bevæger sig op på Application-niveau og kigger på de enkelte Command-handlers (state changers), så er det ganske effektivt at kunne kalde sine operationer på sine repositories - og til sidst kalde Uow.Commit();
Når det kommer til Query-handlers (rene læsninger), så står man med et dilemma. Et repository vil returnere entities - men hvis man f.eks. ønsker at have en handler som returnerer en sammensat viewmodel, så skal man pludselig hive rigtig meget data ud af databasen for at kunne sammensætte det ønskede resultat. Dette taler for at have sin Dapper-kode direkte i Query-handleren, da man så vil kunne tilpasse sin SQL-query til de nødvendige data. Dette vil dog blive på bekostning af separation.
Alternativt skal man lade sine viewmodels bløde helt ud på infrastructure-laget, men så ender man med en løsning der er tæt koblet i stedet. 

Et andet alternativ vil være at droppe brugen af Dapper og i stedet gør brug af EF direkte ude i query-handlers og kombinere dette med projections (enten via AutoMapper eller direkte som POCO-extensions). Dette vil give en stor fleksibilitet og minimere "data-spild", men igen på bekostning af separation. 
Endvidere skal man være obs på at alle skal være opmærksomme på at optimere deres read-queries fordi der er direkte adgang til DbContext. Så ting som "AsNoTracking" bliver noget man aktivt skal huske på. Endvidere risikerer man at forretningslogik skal replikeres til data-forespørgsler (som f.eks. "Hent alle aktive trademarks" - men hvad ligger der i "aktive"?)
En mitigering af dette kunne f.eks. være at implementere et Specification pattern. Dette er relativt let i EF pga. LINQ-to-SQL oversætteren. Med Dapper vil dette være en helt anden og meget større opgave.  
  
## Domain events
I forbindelse med at skabe en løst koblet løsning (samt i forberedelse til evt. at skulle splitte løsningen op i flere mindre bounded contexts), er der implementeret domain events. I forbindelse med DDD-operationer kan ligges events på den enkelte entitets kø. I forbindelse med UoW commit, loopes evt. event igennem og sendes ud på en kø. Dette kan f.eks. dække: 
  - MediatR til intern brug i løsningen
  - MassTransit til at sende beskeder via f.eks. RabbitMq eller Azure Service Bus (til brug i et setup med flere bounded contexts)


## MediatR
I løsningen er pt. benyttet et mediator pattern implementeret igennem MediatR-pakken. Dette skaber en meget løs kobling i systemet - ligesom det passer perfekt ind i et CQRS-light / Clean Architecture setup. Ud af æsken for man også separation of concerns forærende. 
Endvidere har MediatR en indbygget behavior pipeline der gør det nemt at implementere performance-monitoring på enkelte requests, fluent validation etc.
  Den løse kobling kommer dog på bekostning af f.eks. debugging. Fordi der ikke er nogen direkte binding - men alt går igennem mediatoren - er man nødt til at navigere vha. "Go to implementation", "Find usages" samt kendt navne-konvention. 
  Dette virker som et let tradeoff - men bør nok overvejes i forhold til løsninger med MANGE commands og queries. 
  
  ## Andre overvejelser
  Jeg er tiltalt af hastigheden og de rå SQL ved Dapper. Udfordringen er dog at det netop er et mikro-ORM - og dermed skal man håndbære en del andre ting selv. 
  I den anden grøft er EF en tung dame at danse med (dog er det blevet MEGET bedre med omskrivningen til EF Core). Her skal man være meget obs på sine executions, optimering af queries etc. ligesom der er et uundgåeligt performance overhead til pipeline, mapping etc. 
  Omvendt er det super nemt at implementere domain events samt håndtere nem interface segregation til ting som f.eks. soft deletes, auditable entities osv. Her vil man blot kigge på om ændrede entiteter i SaveChanges() implementerer et eller flere interfaces - og så tilpasse sine entiteter tilsvarende inden save. 
  
  Dermed udestår spørgsmål stadig: Med de tradeoffs det giver, er det så dét værd at implementere 2 forskellige data providers separeret via query og command repositories? Det kommer nok an på løsningen. Simplere løsninger med få udviklere vil nok få større glæde af at have datalogik direkte i query handlers (om ikke andet læsningerne i hvert fald). For store løsninger er der et risk i at det nemt bliver det vilde vesten. Det kræver om ikke andet diciplin. 
