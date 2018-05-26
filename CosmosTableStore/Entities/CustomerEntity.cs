using Microsoft.Azure.CosmosDB.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosTableStore.Entities
{
    public class CustomerEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Your entity type must expose a parameter-less constructor
        /// </summary>
        public CustomerEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Defines the PK and RK.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <param name="firstName">The first name.</param>
        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        /// <summary>
        /// Gets or sets the email address for the customer.
        /// A property for use in Table storage must be a public property of a 
        /// supported type that exposes both a getter and a setter.        
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number for the customer.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }
    }
}
