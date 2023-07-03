What i would do more:
- fix CreditLimitCalculatorFactory (does not filter types correctly)
- improve folders structure (move code files to separate folders) - example: credit limit calculators and factory, services - if possible and does not brake requirements
- fix namespaces after moving files to folders

Other things:
- UserCreditService - if creating it has some side effects my code would break existing codebase, but if it is happenning then there is a deeper problem with this service - it's creation should not have side effects
- Dependency Injection - would improve creating instances of types, but it would break other code