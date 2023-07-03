What i would do more:
- fix CreditLimitCalculatorFactory (does not filter types correctly)
- improve folders structure (move code files to separate folders) - example: credit limit calculators and factory, services - if possible and does not brake requirements
- fix namespaces after moving files to folders

Other things:
- UserCreditService - if creating it has some side effects my code would break existing codebase, but if it is happenning then there is a deeper problem with this service - it's creation should not have side effects
- Dependency Injection - would improve creating instances of types, but it would break other code

Another improvement i would have not written when showing results of the tests:
- IUserService was not needed so i deleted it
- there are other things like using some validation library (fluent validations) - did not do because time was limited and doing it would introduce new way of handling validations - possible conflict with project's common way of doing it - opinion of others needed, addind dto for user data but in the context of the test doing it is impossible

Things that interviewer checking results of my test may mark as wrong:
- i changed namespaces of UserCreditService and UserDataAccess - it may not necessarily be wrong but first one is generated, second one is static so it can have impact on other legacy code, however README.md stated that in UserDataAccess only static method and static class should not change