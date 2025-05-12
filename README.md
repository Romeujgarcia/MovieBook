Estrutura do Sistema de Reserva de Filmes
Vamos usar uma arquitetura em camadas seguindo os princípios de Clean Architecture:
MovieBookingSystem/
├── MovieBookingSystem.API              (API REST)
├── MovieBookingSystem.Application      (Lógica de negócio)
├── MovieBookingSystem.Domain           (Entidades e regras de domínio)
├── MovieBookingSystem.Infrastructure   (Acesso a dados, serviços externos)
└── MovieBookingSystem.Tests            (Testes unitários e integração)


https://roadmap.sh/projects/movie-reservation-system
