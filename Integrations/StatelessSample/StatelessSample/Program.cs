using Stateless;
using System;
using System.Collections.Generic;

namespace StatelessSample {

    class Program {

        static void Main(string[] args) {

            Product product = new Product {

                Id = 1,
                Name = "Microsoft Surface",
                Status = ProductStatus.Draft
            };

            Console.WriteLine("Allowed Triggers:");
            foreach (ProductStatus permittedTrigger in product.PermittedProductStatusTypes) {

                Console.WriteLine(permittedTrigger);
            }

            // Try to change the Status to a valid state
            product.Status = ProductStatus.WaitingForApproval;

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("The product state is now {0}.", product.Status);

            try {

                // Try to change the Status to an invalid state
                product.Status = ProductStatus.Active;
            }
            catch (InvalidOperationException ex) {

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }

    public class Product {

        private StateMachine<ProductStatus, ProductStatus> _productStateMachine;

        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<ProductStatus> PermittedProductStatusTypes { 

            get {

                return _productStateMachine.PermittedTriggers;
            } 
        }

        public ProductStatus Status {

            get {

                return _productStateMachine.State;
            }

            set {

                if (_productStateMachine == null) {

                    _productStateMachine = InitializeStateMachine(value);
                }
                else {

                    _productStateMachine.Fire(value);
                }
            } 
        }

        private static StateMachine<ProductStatus, ProductStatus> InitializeStateMachine(ProductStatus initialState) {

            StateMachine<ProductStatus, ProductStatus> productStateMachine =
                new StateMachine<ProductStatus, ProductStatus>(initialState);

            productStateMachine.Configure(ProductStatus.Draft)
                .Permit(ProductStatus.WaitingForApproval, ProductStatus.WaitingForApproval);

            productStateMachine.Configure(ProductStatus.WaitingForApproval)
                .Permit(ProductStatus.Approved, ProductStatus.Approved)
                .Permit(ProductStatus.Cancelled, ProductStatus.Cancelled);

            productStateMachine.Configure(ProductStatus.Approved)
                .Permit(ProductStatus.Active, ProductStatus.Active)
                .Permit(ProductStatus.Passive, ProductStatus.Passive)
                .Permit(ProductStatus.Cancelled, ProductStatus.Cancelled);

            productStateMachine.Configure(ProductStatus.Active)
                .Permit(ProductStatus.Passive, ProductStatus.Passive)
                .Permit(ProductStatus.Cancelled, ProductStatus.Cancelled);

            productStateMachine.Configure(ProductStatus.Passive)
                .Permit(ProductStatus.Active, ProductStatus.Active)
                .Permit(ProductStatus.Cancelled, ProductStatus.Cancelled);

            // Allow none of the options for AccommodationPropertyStatus.Cancelled;

            return productStateMachine;
        }
    }

    public enum ProductStatus : byte { 

        Draft = 1,
        WaitingForApproval = 2,
        Approved = 3,
        Active = 4,
        Passive = 5,
        Cancelled = 6
    }
}