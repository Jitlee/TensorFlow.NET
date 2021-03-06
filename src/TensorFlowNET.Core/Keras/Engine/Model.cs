﻿using static Tensorflow.Binding;
using System;
using Tensorflow.Keras.ArgsDefinition;
using Tensorflow.Keras.Engine.DataAdapters;
using Tensorflow.Keras.Losses;
using Tensorflow.Keras.Optimizers;
using NumSharp;

namespace Tensorflow.Keras.Engine
{
    /// <summary>
    /// `Model` groups layers into an object with training and inference features.
    /// </summary>
    public partial class Model : Layer
    {
#pragma warning disable CS0169 // The field 'Model._cloning' is never used
        bool _cloning;
#pragma warning restore CS0169 // The field 'Model._cloning' is never used
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
#pragma warning disable CS0414 // The field 'Model._is_compiled' is assigned but its value is never used
        bool _is_compiled;
#pragma warning restore CS0414 // The field 'Model._is_compiled' is assigned but its value is never used
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        ILossFunc loss;
        IOptimizer optimizer;
        IVariableV1 _steps_per_execution;
        protected bool _is_graph_network;
        protected Tensors inputs;
        protected Tensors outputs;
        public string[] output_names;
        IVariableV1 _train_counter;
        IVariableV1 _test_counter;
        IVariableV1 _predict_counter;
        bool _base_model_initialized;

        public Model(ModelArgs args) 
            : base(args)
        {
            
        }

        public void compile(ILossFunc loss, OptimizerV2 optimizer, string[] metrics)
        {
            this.optimizer = optimizer;
            var compiled_loss = new LossesContainer(loss, output_names: output_names);
            var compiled_metrics = new MetricsContainer(metrics, output_names: output_names);

            int experimental_steps_per_execution = 1;
            _configure_steps_per_execution(experimental_steps_per_execution);

            // Initialize cache attrs.
            _reset_compile_cache();
            _is_compiled = true;
            this.loss = loss;
        }

        public void compile(string optimizerName, string lossName)
        {
            switch (optimizerName)
            {
                case "rmsprop":
                    optimizer = new RMSprop(new RMSpropArgs
                    {

                    });
                    break;
            }

            int experimental_steps_per_execution = 1;
            _configure_steps_per_execution(experimental_steps_per_execution);

            _reset_compile_cache();

            _is_compiled = true;
        }

        /// <summary>
        /// Trains the model for a fixed number of epochs (iterations on a dataset).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="batch_size"></param>
        /// <param name="epochs"></param>
        /// <param name="verbose"></param>
        /// <param name="validation_split"></param>
        /// <param name="shuffle"></param>
        public void fit(NDArray x, NDArray y,
            int batch_size = -1,
            int epochs = 1,
            int verbose = 1,
            float validation_split = 0f,
            bool shuffle = true,
            int initial_epoch = 0,
            int max_queue_size = 10,
            int workers = 1,
            bool use_multiprocessing = false)
        {
            int train_count = Convert.ToInt32(x.shape[0] * (1 - validation_split));
            var train_x = x[new Slice(0, train_count)];
            var train_y = y[new Slice(0, train_count)];
            var val_x = x[new Slice(train_count)];
            var val_y = y[new Slice(train_count)];

            var data_handler = new DataHandler(new DataHandlerArgs
            {
                X = train_x,
                Y = train_y,
                BatchSize = batch_size,
                InitialEpoch = initial_epoch,
                Epochs = epochs,
                Shuffle = shuffle,
                MaxQueueSize = max_queue_size,
                Workers = workers,
                UseMultiprocessing = use_multiprocessing,
                Model = this,
                StepsPerExecution = _steps_per_execution
            });
        }

        void _configure_steps_per_execution(int steps_per_execution)
        {
            _steps_per_execution = tf.Variable(steps_per_execution,
                dtype: TF_DataType.TF_INT64,
                aggregation: VariableAggregation.OnlyFirstReplica);
        }

        void _reset_compile_cache()
        {
            // Used to cache `trainable` attr of `Layer`s for `fit`.
            _compiled_trainable_state = _get_trainable_state();
        }

        void _init_batch_counters()
        {
            _train_counter = tf.Variable(0, 
                dtype: TF_DataType.TF_INT64, 
                aggregation: VariableAggregation.OnlyFirstReplica);

            _test_counter = tf.Variable(0,
                dtype: TF_DataType.TF_INT64,
                aggregation: VariableAggregation.OnlyFirstReplica);

            _predict_counter = tf.Variable(0,
                dtype: TF_DataType.TF_INT64,
                aggregation: VariableAggregation.OnlyFirstReplica);
        }

        public void compile(string optimizerName, ILossFunc lossName)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// Generates output predictions for the input samples.
        /// </summary>
        /// <param name="x">Input samples</param>
        /// <param name="batch_size">Number of samples per batch</param>
        /// <param name="verbose">Verbosity mode</param>
        /// <param name="steps">
        /// Total number of steps (batches of samples)
        /// before declaring the prediction round finished.
        /// </param>
        /// <param name="max_queue_size"></param>
        /// <param name="workers"></param>
        /// <param name="use_multiprocessing"></param>
        /// <returns></returns>
        public Tensor predict(Tensor x,
            int batch_size = 32,
            int verbose = 0,
            int steps = -1,
            int max_queue_size = 10,
            int workers = 1,
            bool use_multiprocessing = false)
        {
            var data_handler = new DataHandler(new DataHandlerArgs
            {
                X = x,
                BatchSize = batch_size,
                StepsPerEpoch = steps,
                InitialEpoch = 0,
                Epochs = 1,
                MaxQueueSize = max_queue_size,
                Workers = workers,
                UseMultiprocessing = use_multiprocessing,
                Model = this,
                StepsPerExecution = _steps_per_execution
            });

            throw new NotImplementedException("");
        }
    }
}
