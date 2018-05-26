namespace CosmosTableRepository.Exception
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ClientNotInitializedException : Exception
    {
        /// <summary>
        /// The default message
        /// </summary>
        private static string DefaultMessage = "Document Db Client is not initialized";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNotInitializedException"/> class.
        /// </summary>
        public ClientNotInitializedException() : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNotInitializedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ClientNotInitializedException(string message) : base(message)
        {
            DefaultMessage = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNotInitializedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ClientNotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
            DefaultMessage = message;
        }

        /// <summary>
        /// The get object data
        /// To get the data related to serialization of object
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("DefaultMessage", DefaultMessage);
        }
    }
}
